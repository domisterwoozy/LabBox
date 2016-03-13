using LabBox.Visualization.Input;
using Math3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    /// <summary>
    /// A camera controlled by user input.
    /// </summary>
    public class FreeCamera : ICamera
    {
        private const int PollsPerSecond = 144;
        private static readonly TimeSpan interval = TimeSpan.FromSeconds(1.0 / PollsPerSecond);

        private Timer inputPollTimer = null;

        public float VertFOV { get; set; } = (float)Math.PI / 3;
        public float MaxRange { get; set; } = 70.0f;
        public float MinRange { get; set; } = 0.1f;
        public float AspectRatio { get; } = 16.0f / 9.0f;

        public Vector3 Pos { get; private set; } = new Vector3(10, 10, 10);
        public Vector3 UpDir { get; private set; } = Vector3.K;
        public Vector3 LookAtPos { get; private set; } = Vector3.Zero;

        /// <summary>
        /// The input handler used to control this camera.
        /// </summary>
        public IInputHandler CameraInput { get; }
        /// <summary>
        /// Use to tune the lateral movement speed of the camera.
        /// </summary>
        public float MoveSpeed { get; set; } = 10.0f;
        /// <summary>
        /// Use to tune the rotation speed of the camera.
        /// </summary>
        public float TurnSpeed { get; set; } = 0.25f;
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

        public FreeCamera(IInputHandler camInput)
        {
            CameraInput = camInput;
            camInput.TurnLeft.Start += TurnLeft_Start;
            camInput.TurnRight.Start += TurnRight_Start;
            camInput.TurnUp.Start += TurnUp_Start;
            camInput.TurnDown.Start += TurnDown_Start;
        }       

        private void StartPollHeartBeat()
        {
            inputPollTimer = new Timer(PollInput, null, interval, interval);
        }

        private void StopPollHeartBeat()
        {
            inputPollTimer.Change(Timeout.Infinite, Timeout.Infinite);
            inputPollTimer = null;
        }

        private Vector3 HorizontalForward() => (LookAtPos - Pos).ProjectToPlane(Vector3.K).UnitDirection;
        private Vector3 HorizontalRight() => HorizontalForward() % Vector3.K;

        private void PollInput(object timerState)
        {
            // move forward and back
            if (CameraInput.Forward.IsActive)
            {
                Pos += (1.0 / PollsPerSecond) * MoveSpeed * HorizontalForward();
                LookAtPos += (1.0 / PollsPerSecond) * MoveSpeed * HorizontalForward();
            }
            if (CameraInput.Backward.IsActive)
            {
                Pos += (1.0 / PollsPerSecond) * MoveSpeed * -HorizontalForward();
                LookAtPos += (1.0 / PollsPerSecond) * MoveSpeed * -HorizontalForward();
            }

            // move left and right
            if (CameraInput.Left.IsActive)
            {
                Pos += (1.0 / PollsPerSecond) * MoveSpeed * -HorizontalRight();
                LookAtPos += (1.0 / PollsPerSecond) * MoveSpeed * -HorizontalRight();
            }
            if (CameraInput.Right.IsActive)
            {
                Pos += (1.0 / PollsPerSecond) * MoveSpeed * HorizontalRight();
                LookAtPos += (1.0 / PollsPerSecond) * MoveSpeed * HorizontalRight();
            }

            // float up and down
            if (CameraInput.Up.IsActive)
            {
                Pos += (1.0 / PollsPerSecond) * MoveSpeed * Vector3.K;
                LookAtPos += (1.0 / PollsPerSecond) * MoveSpeed * Vector3.K;
            }
            if (CameraInput.Down.IsActive)
            {
                Pos -= (1.0 / PollsPerSecond) * MoveSpeed * Vector3.K;
                LookAtPos -= (1.0 / PollsPerSecond) * MoveSpeed * Vector3.K;
            }

            // turning
            if (CameraInput.TurnUp.IsActive)
            {
                LookAtPos += (1.0 / PollsPerSecond) * CameraInput.TurnUp.Weight * TurnSpeed * Vector3.K;
            }
            if (CameraInput.TurnDown.IsActive)
            {
                LookAtPos -= (1.0 / PollsPerSecond) * CameraInput.TurnDown.Weight * TurnSpeed * Vector3.K;
            }
            if (CameraInput.TurnLeft.IsActive)
            {
                LookAtPos += (1.0 / PollsPerSecond) * CameraInput.TurnLeft.Weight * TurnSpeed * -HorizontalRight();
            }
            if (CameraInput.TurnRight.IsActive)
            {
                LookAtPos += (1.0 / PollsPerSecond) * CameraInput.TurnRight.Weight * TurnSpeed * HorizontalRight();
            }
        }


        private void TurnLeft_Start(object sender, EventArgs e)
        {
            if (IsLocked) return;
            LookAtPos += (1.0 / PollsPerSecond) * CameraInput.TurnLeft.Weight * TurnSpeed * -HorizontalRight();
        }

        private void TurnRight_Start(object sender, EventArgs e)
        {
            if (IsLocked) return;
            LookAtPos += (1.0 / PollsPerSecond) * CameraInput.TurnRight.Weight * TurnSpeed * HorizontalRight();
        }        

        private void TurnUp_Start(object sender, EventArgs e)
        {
            if (IsLocked) return;
            LookAtPos += (1.0 / PollsPerSecond) * CameraInput.TurnUp.Weight * TurnSpeed * Vector3.K;
        }

        private void TurnDown_Start(object sender, EventArgs e)
        {
            if (IsLocked) return;
            LookAtPos -= (1.0 / PollsPerSecond) * CameraInput.TurnDown.Weight * TurnSpeed * Vector3.K;
        }
    }
}
