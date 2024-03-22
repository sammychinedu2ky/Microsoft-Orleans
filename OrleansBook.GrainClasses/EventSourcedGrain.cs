using Orleans.EventSourcing;
using Orleans.Providers;
using OrleansBook.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBook.GrainClasses
{
    [StorageProvider(ProviderName = "robotStateStore")]
    public class EventSourcedGrain : JournaledGrain<EventSourcedState,
   IEvent>, IRobotGrain
    {
        public async Task AddInstruction(string instruction)
        {
            RaiseEvent(new EnqueueEvent(instruction));
            await ConfirmEvents();
        }
        public async Task<string> GetNextInstruction()
        {
            if (this.State.Count == 0) return null;
            var @event = new DequeueEvent();
            RaiseEvent(@event);
            await ConfirmEvents();
            return @event.Value;
        }
        public Task<int> GetInstructionCount() => Task.FromResult(this.State.Count);
    }
}