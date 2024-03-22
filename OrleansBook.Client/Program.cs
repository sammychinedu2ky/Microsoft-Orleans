using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Streams;
using OrleansBook.GrainInterfaces;
namespace OrleansBook.Client
{
    class Program
    {
        static async Task Main()
        {
            using IHost host = new HostBuilder()
      .UseOrleansClient(clientBuilder =>
      {
          clientBuilder.UseLocalhostClustering().AddMemoryStreams("StreamProvider");
      })
      .Build();
            await host.StartAsync();

            IClusterClient client = host.Services.GetRequiredService<IClusterClient>();
            // var grain = client.GetGrain<IRobotGrain>("rob1");
            // var id =  grain.GetPrimaryKeyString();
            var subHandle = await client.GetStreamProvider("StreamProvider")
                .GetStream<InstructionMessage>("MyNameSpace", "rob1").SubscribeAsync(new StreamObserver());
            await subHandle.ResumeAsync(new StreamObserver());
            while (true)
            {
                Console.WriteLine("Please enter a robot name...");
                try
                {
                    var grainId = Console.ReadLine();
                    var grain = client.GetGrain<IRobotGrain>(grainId);
                    Console.WriteLine("Please enter an instruction...");
                    var instruction = Console.ReadLine()!;
                    await grain.AddInstruction(instruction);
                    var count = await grain.GetInstructionCount();
                    Console.WriteLine($"{grainId} has {count} instruction(s)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }

    public class StreamObserver : IAsyncObserver<InstructionMessage>
    {
        public Task OnCompletedAsync()
        {
            Console.WriteLine("Completed");
            return Task.CompletedTask;
        }
        public Task OnErrorAsync(Exception ex)
        {
            Console.WriteLine("Exception");
            Console.WriteLine(ex.ToString());
            return Task.CompletedTask;
        }
        public Task OnNextAsync(
        InstructionMessage instruction,
        StreamSequenceToken token = null)
        {
            var msg = $"{instruction.Robot} starting \"{instruction.
            Instruction}\"";
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
    }
}


