using Orleans;
namespace OrleansBook.GrainInterfaces
{
    public interface ISubscriberGrain : IGrainWithStringKey
    { }

    public interface IRobotGrain : IGrainWithStringKey
    {
        [Transaction(TransactionOption.CreateOrJoin)]
        Task AddInstruction(string instruction);

        [Transaction(TransactionOption.CreateOrJoin)]
        Task<string> GetNextInstruction();
        
        [Transaction(TransactionOption.CreateOrJoin)]
        Task<int> GetInstructionCount();
    }

    public class StorageValue
    {
        public string Value { get; set; }
    }

    [GenerateSerializer]
    public class InstructionMessage
    {
        public InstructionMessage(string instruction, string robot)
        {
            this.Instruction = instruction;
            this.Robot = robot;
        }

        [Id(0)]
        public string Instruction { get; set; }
        [Id(1)]
        public string Robot { get; set; }
    }
}
