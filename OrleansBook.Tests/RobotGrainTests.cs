using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Orleans.TestingHost;
using Orleans.Transactions.Abstractions;
using OrleansBook.GrainClasses;
using OrleansBook.GrainInterfaces;

namespace OrleansBook.Tests
{
    [TestClass]
    public class RobotGrainTests
    {
        static TestCluster cluster;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            cluster = new TestClusterBuilder()
            .AddSiloBuilderConfigurator<SiloBuilderConfigurator>()
            .Build();
            cluster.Deploy();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            cluster.StopAllSilos();
        }

        [TestMethod]
        public async Task TestInstructions()
        {
            var robot = cluster.GrainFactory.GetGrain<IRobotGrain>("test");
            await robot.AddInstruction("Do the dishes");
            await robot.AddInstruction("Take out the trash");
            await robot.AddInstruction("Do the laundry");
            Assert.AreEqual(3, await robot.GetInstructionCount());
            Assert.AreEqual(
            await robot.GetNextInstruction(), "Do the dishes");
            Assert.AreEqual(
            await robot.GetNextInstruction(), "Take out the trash");
            Assert.AreEqual(
            await robot.GetNextInstruction(), "Do the laundry");
            Assert.IsNull(await robot.GetNextInstruction());
            Assert.AreEqual(0, await robot.GetInstructionCount());
        }
    }

    class SiloBuilderConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            siloBuilder.UseTransactions();
            siloBuilder.AddMemoryGrainStorage("robotStateStore");
            //var mockState = new Mock<IPersistentState<RobotState>>();
            //mockState.SetupGet(s => s.State).Returns(new RobotState());
            var mockState = new Mock<ITransactionalState<RobotState>>();
            var mockRobotState = new RobotState();
            siloBuilder.ConfigureServices(services =>
            {
                //services.AddSingleton<IPersistentState<RobotState>>(mockState.Object);
                services.AddSingleton<ILogger<RobotGrain>>(
                    new Mock<ILogger<RobotGrain>>().Object
                );
            });

        }
    }
}
