using Orleans;
namespace OrleansBook.GrainInterfaces
{
    public interface IRobotGrain : IGrainWithStringKey
    {
        Task AddInstruction(string instruction);
        Task<string> GetNextInstruction();
        Task<int> GetInstructionCount();
    }
}
