using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization.Input
{
    public interface IInput
    {
        event EventHandler Start;
        event EventHandler Finish;
        bool IsActive { get; }
        double Weight { get; }
    }
}
