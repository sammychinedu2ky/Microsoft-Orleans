namespace OrleansBook.GrainInterfaces
{
    public interface IBatchGrain : IGrainWithIntegerKey
    {
        [Transaction(TransactionOption.Create)]
        Task AddInstructions((string, string)[] values);
    }
}
