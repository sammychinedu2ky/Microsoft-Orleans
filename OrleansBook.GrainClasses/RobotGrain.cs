using Microsoft.AspNetCore.Connections.Features;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using OrleansBook.GrainInterfaces;
namespace OrleansBook.GrainClasses
{
    public class RobotGrain : Grain, IRobotGrain
    {
        ILogger<RobotGrain> logger;
        IPersistentState<RobotState> state;
       
        public RobotGrain(ILogger<RobotGrain> logger, [PersistentState("robotState", "robotStore")] IPersistentState<RobotState> state)
        {
            this.logger = logger;
            this.state = state;
        }
        private Queue<string> instructions = new Queue<string>();
        public async Task AddInstruction(string instruction)
        {
            var key = this.GetPrimaryKeyString();
            this.logger.LogWarning("{Key} adding '{Instruction}'",
            key, instruction);
            //this.instructions.Enqueue(instruction);
            this.state.State.Instructions.Enqueue(instruction);
            await this.state.WriteStateAsync();
          //  return Task.CompletedTask;
        }
        public Task<int> GetInstructionCount()
        {
            return Task.FromResult(this.state.State.Instructions.Count);

            //return Task.FromResult(this.instructions.Count);
        }
        public async Task<string> GetNextInstruction()
        {
            if (this.state.State.Instructions.Count == 0)
            {
                return null;
            }
            var instruction = this.state.State.Instructions.Dequeue();
            var key = this.GetPrimaryKeyString();
            this.logger.LogWarning("{Key} adding '{Instruction}'",
            key, instruction);
            await this.state.WriteStateAsync();
            return instruction;
        }
    }
}
