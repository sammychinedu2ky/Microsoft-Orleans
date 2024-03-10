using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHostBuilder builder = new HostBuilder()
    .UseOrleans(silo =>
    {
       silo.UseLocalhostClustering();
       silo.UseDashboard(options =>
       {
           options.Port = 8083;
       })
       .ConfigureLogging(logging => logging.AddConsole());
       
      
    })
    
    .UseConsoleLifetime();

using IHost host = builder.Build();

await host.RunAsync();
