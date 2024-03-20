using Microsoft.AspNetCore.Http.HttpResults;
using Orleans.Runtime;
using OrleansBook.GrainInterfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseOrleans(static siloBuilder =>
{
  siloBuilder.UseLocalhostClustering();
       siloBuilder.UseDashboard(options =>
       {
           options.Port = 8083;
       })
       .ConfigureLogging(logging => logging.AddConsole())
       .AddAzureTableGrainStorage("robotStateStore", options=>{
            options.ConfigureTableServiceClient("UseDevelopmentStorage=true");
       })
       .AddMemoryStreams("StreamProvider")
     .AddMemoryGrainStorage("PubSubStore")
     .UseInMemoryReminderService()
     .UseTransactions();

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/robot/{name}/instruction", (IGrainFactory grain, string name) =>
{
    var robot = grain.GetGrain<IRobotGrain>(name);
    return robot.GetNextInstruction();
});
app.MapPost("/robot/{name}/instruction", async (IGrainFactory grain, string name, StorageValue value) =>
{
    var robot = grain.GetGrain<IRobotGrain>(name);
    await robot.AddInstruction(value.Value);
    return Results.Ok();
});
app.MapPost("/batch", async (IGrainFactory grain, IDictionary<string,string> values) =>
{
   var batchGrain = grain.GetGrain<IBatchGrain>(0);
   var input = values.Select(keyValue => (keyValue.Key, keyValue.Value)).ToArray();
   await batchGrain.AddInstructions(input);
   return Results.Ok();
});

app.Run();

