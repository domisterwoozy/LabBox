using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Input
{
    public interface IInputObservable
    {
        IObservable<IInput> InputEvents { get; }
    }
}
