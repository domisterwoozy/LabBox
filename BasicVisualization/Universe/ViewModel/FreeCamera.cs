using LabBox.Visualization.Input;
using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace LabBox.Visualization.Universe.ViewModel
{
    /// <summary>
    /// A camera controlled by user input.
    /// </summary>
    public class FreeCamera : ICamera, IDisposable
    {
        private const int PollsPerSecond = 144;
        private static readonly TimeSpan interval = TimeSpan.FromSeconds(1.0 / PollsPerSecond);
        private Timer inputPollTimer = null;

        private readonly List<IDisposable> subs = new List<IDisposable>();

        public float VertFOV { get; set; } = (float)Math.PI / 3;
        public float MaxRange { get; set; } = 30.0f;
        public float MinRange { get; set; } = 0.1f;
        public float AspectRatio { get; } = 16.0f / 9.0f;

        public Vector3 Pos { get; private set; } = new Vector3(2, 2, 2);
        public Vector3 UpDir { get; private set; } = Vector3.J;
        public Vector3 LookAtPos { get; private set; } = Vector3.Zero;

        /// <summary>
        /// The input handler used to control this camera.
        /// </summary>
        public IInputObservable CameraInput { get; }
        /// <summary>
        /// Use to tune the lateral movement speed of the camera.
        /// </summary>
        public float MoveSpeed { get; set; } = 10.0f;
        /// <summary>
        /// Use to tune the rotation speed of the camera.
        /// </summary>
        public float TurnSpeed { get; set; } = 0.0025f;
        /// <summary>
        /// Whether the input controlled camera is currently accepting input.
        /// </summary>
        public bool IsLocked
        {
            get { return inputPollTimer == null; }
            set
            {
                if (value) StopPollHeartBeat();
                else StartPollHeartBeat();
            }
        }

        public FreeCamera(IInputObservable camInput)
        {
            CameraInput = camInput;

            // all inputs when were not locked
            var inputs = from evt in camInput.InputEvents where !IsLocked select evt;

            // handle look inputs
            var turnLeftInputs = from inpt in inputs where inpt.Input == InputType.TurnLeft select inpt;
            var turnRightInputs = from inpt in inputs where inpt.Input == InputType.TurnRight select inpt;
            var turnUpInputs = from inpt in inputs where inpt.Input == InputType.TurnUp select inpt;
            var turnDownInputs = from inpt in inputs where inpt.Input == InputType.TurnDown select inpt;
            subs.Add(turnLeftInputs.Subscribe(inpt => MoveLookAtPos(inpt.Weight * TurnSpeed * -HorizontalRight())));
            subs.Add(turnRightInputs.Subscribe(inpt => MoveLookAtPos(inpt.Weight * TurnSpeed * HorizontalRight())));
            subs.Add(turnUpInputs.Subscribe(inpt => MoveLookAtPos(inpt.Weight * TurnSpeed * Vector3.J)));
            subs.Add(turnDownInputs.Subscribe(inpt => MoveLookAtPos(inpt.Weight * TurnSpeed * -Vector3.J)));

            // handle movement inputs
            var fwdInputs = from inpt in inputs where inpt.Input == InputType.Forward select inpt;
            subs.Add(fwdInputs.Where(inpt => inpt.State == InputState.Start).Subscribe(inpt => StartMovement(MovementDir.Forward)));
            subs.Add(fwdInputs.Where(inpt => inpt.State == InputState.Finish).Subscribe(inpt => EndMovement(MovementDir.Forward)));
            var bwdInputs = from inpt in inputs where inpt.Input == InputType.Backward select inpt;
            subs.Add(bwdInputs.Where(inpt => inpt.State == InputState.Start).Subscribe(inpt => StartMovement(MovementDir.Backward)));
            subs.Add(bwdInputs.Where(inpt => inpt.State == InputState.Finish).Subscribe(inpt => EndMovement(MovementDir.Backward)));
            var leftInputs = from inpt in inputs where inpt.Input == InputType.Left select inpt;
            subs.Add(leftInputs.Where(inpt => inpt.State == InputState.Start).Subscribe(inpt => StartMovement(MovementDir.Left)));
            subs.Add(leftInputs.Where(inpt => inpt.State == InputState.Finish).Subscribe(inpt => EndMovement(MovementDir.Left)));
            var rightInputs = from inpt in inputs where inpt.Input == InputType.Right select inpt;
            subs.Add(rightInputs.Where(inpt => inpt.State == InputState.Start).Subscribe(inpt => StartMovement(MovementDir.Right)));
            subs.Add(rightInputs.Where(inpt => inpt.State == InputState.Finish).Subscribe(inpt => EndMovement(MovementDir.Right)));
            var upInputs = from inpt in inputs where inpt.Input == InputType.Up select inpt;
            subs.Add(upInputs.Where(inpt => inpt.State == InputState.Start).Subscribe(inpt => StartMovement(MovementDir.Up)));
            subs.Add(upInputs.Where(inpt => inpt.State == InputState.Finish).Subscribe(inpt => EndMovement(MovementDir.Up)));
            var downInputs = from inpt in inputs where inpt.Input == InputType.Down select inpt;
            subs.Add(downInputs.Where(inpt => inpt.State == InputState.Start).Subscribe(inpt => StartMovement(MovementDir.Down)));
            subs.Add(downInputs.Where(inpt => inpt.State == InputState.Finish).Subscribe(inpt => EndMovement(MovementDir.Down)));
        }

        public void Dispose()
        {
            foreach (var sub in subs) sub.Dispose();
        }

        private Vector3 HorizontalForward() => (LookAtPos - Pos).ProjectToPlane(Vector3.J).UnitDirection;
        private Vector3 HorizontalRight() => HorizontalForward() % Vector3.J;
        private void MoveLookAtPos(Vector3 amtToMove)
        {
            LookAtPos += amtToMove;
        }
        private void MovePos(Vector3 amtToMove)
        {
            Pos += amtToMove;
        }
        private void Move(Vector3 amtToMove)
        {
            MoveLookAtPos(amtToMove);
            MovePos(amtToMove);
        }

        #region Polling to smooth out movement
        private void StartPollHeartBeat()
        {
            inputPollTimer = new Timer(PollInput, null, interval, interval);
        }

        private void StopPollHeartBeat()
        {
            inputPollTimer.Change(Timeout.Infinite, Timeout.Infinite);
            inputPollTimer = null;
        }
        private void PollInput(object timerState)
        {
            if (currentMovement.Contains(MovementDir.Forward)) Move((1.0 / PollsPerSecond) * MoveSpeed * HorizontalForward());
            if (currentMovement.Contains(MovementDir.Backward)) Move((1.0 / PollsPerSecond) * MoveSpeed * -HorizontalForward());
            if (currentMovement.Contains(MovementDir.Left)) Move((1.0 / PollsPerSecond) * MoveSpeed * -HorizontalRight());
            if (currentMovement.Contains(MovementDir.Right)) Move((1.0 / PollsPerSecond) * MoveSpeed * HorizontalRight());
            if (currentMovement.Contains(MovementDir.Up)) Move((1.0 / PollsPerSecond) * MoveSpeed * Vector3.J);
            if (currentMovement.Contains(MovementDir.Down)) Move((1.0 / PollsPerSecond) * MoveSpeed * -Vector3.J);
        }

        private enum MovementDir { Forward, Backward, Left, Right, Up, Down }
        private HashSet<MovementDir> currentMovement = new HashSet<MovementDir>();
        private void StartMovement(MovementDir dir)
        {
            currentMovement.Add(dir);
        }
        private void EndMovement(MovementDir dir)
        {
            currentMovement.Remove(dir);
        }
        #endregion
    }
}
