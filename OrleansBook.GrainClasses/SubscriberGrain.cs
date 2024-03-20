using Orleans.Streams;
using OrleansBook.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBook.GrainClasses
{
    [ImplicitStreamSubscription("MyNameSpace")]
    public class SubscriberGrain : Grain,
ISubscriberGrain,
IAsyncObserver<InstructionMessage>
    {
        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await this.GetStreamProvider("StreamProvider")
            .GetStream<InstructionMessage>("MyNameSpace", this.GetPrimaryKeyString())
            .SubscribeAsync(this);
            await base.OnActivateAsync(cancellationToken);
        }

        public Task OnNextAsync(
        InstructionMessage instruction,
        StreamSequenceToken token = null)
        {
            var msg = $"{instruction.Robot} starting \"{instruction.Instruction}\"";
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
        public Task OnCompletedAsync() =>
Task.CompletedTask;

        public Task OnErrorAsync(Exception ex) => Task.CompletedTask;
    }

}
