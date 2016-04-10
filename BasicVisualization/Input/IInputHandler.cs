using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Input
{
    /// <summary>
    /// Encapsulates the handling of user input.
    /// It is fed input through an observable and can either return relavant information or produce side effects.
    /// Can be temperarily turned on and off using the Enabled proeprty.
    /// Once it is disposed it completely stops recieving and responding to input.
    /// </summary>
    public interface IInputHandler : IDisposable
    {
        IInputObservable Input { get; }
        bool Enabled { get; set; }
    }
}
