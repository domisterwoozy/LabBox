using System;

namespace LabBox.Visualization.Input
{
    public interface IInput
    {
        InputState State { get; }
        InputType Input { get; }
        double Weight { get; }
    }

    public enum InputState
    {
        Start, Finish, Maintained
    }

    public enum InputType
    {
        // Selection
        PrimarySelect, SecondarySelect, MultiSelect,

        // Movement
        Forward, Backward, Left, Right,
        Up, Down,
        TurnLeft, TurnRight, TurnUp, TurnDown,

        // State
        SpeedUp, SlowDown,
        Exit, Pause
    }

    public struct BasicInput : IInput
    {
        public InputType Input { get; }
        public InputState State { get; }
        public double Weight { get; }

        public BasicInput(InputType input, InputState state, double weight)
        {
            Input = input;
            State = state;
            Weight = weight;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BasicInput))
                return false;
            var typedObj = (BasicInput)obj;
            if (!State.Equals(typedObj.State))
                return false;
            if (!Weight.Equals(typedObj.Weight))
                return false;
            if (!Input.Equals(typedObj.Input))
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode * 23 + State.GetHashCode();
            hashCode = hashCode * 23 + Weight.GetHashCode();
            hashCode = hashCode * 23 + Input.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"State: '{State}'; Weight: '{Weight}'; Input: '{Input}';";
        }

        public BasicInput WithState(InputState newState) => new BasicInput(Input, newState, Weight);
        public BasicInput WithWeight(double newWeight) => new BasicInput(Input, State, newWeight);
        public BasicInput WithInput(InputType newInput) => new BasicInput(newInput, State, Weight);
    }
}
