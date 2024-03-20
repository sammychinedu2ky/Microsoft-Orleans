// using Microsoft.AspNetCore.Connections.Features;
// using Microsoft.Extensions.Logging;
// using Orleans;
// using Orleans.Runtime;
// using Orleans.Streams;
// using Orleans.Reminders;
// using OrleansBook.GrainInterfaces;
// namespace OrleansBook.GrainClasses
// {
//     public class RobotGrain : Grain, IRobotGrain, IRemindable
//     {
//         ILogger<RobotGrain> logger;
//         IPersistentState<RobotState> state;
//         string key;
//         int instructionsEnqueued = 0;
//         int instructionsDequeued = 0;
//         IAsyncStream<InstructionMessage> stream;
//         public RobotGrain(ILogger<RobotGrain> logger, [PersistentState("robotState", "robotStateStore")] IPersistentState<RobotState> state)
//         {
//             this.logger = logger;
//             this.state = state;
//             this.key = this.GetPrimaryKeyString();
//             this.stream = this.GetStreamProvider("StreamProvider")
//             .GetStream<InstructionMessage>("MyNameSpace", key);
//         }

//         Task Publish(string instruction)
//         {
//             var message = new InstructionMessage(
//                 instruction, key
//             );
//             return this.stream.OnNextAsync(message);
//         }
//         private Queue<string> instructions = new Queue<string>();

//         public async Task AddInstruction(string instruction)
//         {
//             var key = this.GetPrimaryKeyString();
//             this.logger.LogWarning("{Key} adding '{Instruction}'",
//             key, instruction);
//             this.state.State.Instructions.Enqueue(instruction);
//             this.instructions.Enqueue(instruction);
//             await this.state.WriteStateAsync();
//             //  return Task.CompletedTask;
//         }
//         public Task<int> GetInstructionCount()
//         {
//             return Task.FromResult(this.state.State.Instructions.Count);

//             //return Task.FromResult(this.instructions.Count);
//         }
//         public async Task<string> GetNextInstruction()
//         {
//             if (this.state.State.Instructions.Count == 0)
//             {
//                 return null;
//             }
//             var instruction = this.state.State.Instructions.Dequeue();
//             var key = this.GetPrimaryKeyString();
//             this.logger.LogWarning("{Key} adding '{Instruction}'",
//             key, instruction);
//             await this.Publish(instruction);
//             this.instructionsDequeued++;
//             await this.state.WriteStateAsync();
//             return instruction;
//         }

//         public async override Task OnActivateAsync(CancellationToken cancellationToken)
//         {
//             var oneMinute = TimeSpan.FromMinutes(1);
//             this.RegisterTimer(this.ResetStats, null, oneMinute, oneMinute);
//             var oneDay = TimeSpan.FromDays(1);
//             await this.RegisterOrUpdateReminder("firmware", oneDay, oneDay);
//             await base.OnActivateAsync(cancellationToken);
//         }
//         Task ResetStats(Object _)
//         {
//             var key = this.GetPrimaryKeyString();
//             Console.WriteLine($"{key} enqueued: {this.instructionsEnqueued}");
//             Console.WriteLine($"{key} dequeued: {this.instructionsDequeued}");
//             Console.WriteLine($"{key} queued: {this.state.State.Instructions.Count}");
//             this.instructionsEnqueued = 0;
//             this.instructionsDequeued = 0;
//             return Task.CompletedTask;
//         }

//         public Task ReceiveReminder(string reminderName, TickStatus status)
//         {
//             if(reminderName == "firmware")
//             {
//                 return this.AddInstruction("Update firmware");
//             }
//             return Task.CompletedTask;
//         }
//     }
// }
