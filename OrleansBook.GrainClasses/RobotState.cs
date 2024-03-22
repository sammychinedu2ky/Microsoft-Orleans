using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansBook.GrainClasses
{
    [GenerateSerializer]
    public class RobotState
    {
        [Id(0)]
        public Queue<string> Instructions { get; set; } = new Queue<string>();
    }
}
