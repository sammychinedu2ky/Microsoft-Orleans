using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBook.GrainClasses
{
    [GenerateSerializer]
    public class EventSourcedState
    {
        [Id(0)]
        Queue<string> instructions = new();
        public int Count => this.instructions.Count;
        public void Apply(EnqueueEvent @event) => this.instructions.Enqueue(@event.Value);
        public void Apply(DequeueEvent @event)
        {
            if(this.instructions.Count == 0) return;
            @event.Value = this.instructions.Dequeue();
        }
    }
}
