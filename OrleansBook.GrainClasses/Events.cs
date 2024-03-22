namespace OrleansBook.GrainClasses
{
    public interface IEvent
    {
    }

    [GenerateSerializer]
    public class EnqueueEvent : IEvent
    {
        [Id(0)]
        public string Value { get; }
        public EnqueueEvent(string value) => this.Value = value;

    }

    [GenerateSerializer]
    public class DequeueEvent : IEvent
    {
        [Id(0)]
        public string Value { get; set; }
        public DequeueEvent() { }
    }
}
