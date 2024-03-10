using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
          clientBuilder.UseLocalhostClustering();
      })
      .Build();
            await host.StartAsync();
            IClusterClient client = host.Services.GetRequiredService<IClusterClient>();
          
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
}


