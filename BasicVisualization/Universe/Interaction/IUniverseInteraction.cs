using LabBox.Visualization.Input;
using Physics3D.Universes;
using System;

namespace LabBox.Visualization.Universe.Interaction
{
    /// <summary>
    /// Encapsulates an interaction b/w user input and the universe.
    /// It is fed input through an observable and can either return relavant information about the universe or produce side effects on the universe.
    /// Can be temperarily turned on and off using the Enabled proeprty.
    /// Once it is disposed and completely stops recieving input.
    /// </summary>
    public interface IUniverseInteraction : IDisposable
    {
        IInputObservable Input { get; }
        IUniverse Uni { get; }
        bool Enabled { get; set; }
    }
}
