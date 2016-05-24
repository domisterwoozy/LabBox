using LabBox.Visualization.Input;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Valve.VR;

namespace HelloVR
{
    internal class OpenVREventArgs : EventArgs
    {
        public EVREventType EventType { get; }
        public int DeviceIndex { get; }
        public float Age { get; }
        public VREvent_Data_t EventData { get; }

        public OpenVREventArgs(VREvent_t rawEvent)
        {
            EventType = (EVREventType)rawEvent.eventType;
            DeviceIndex = (int)rawEvent.trackedDeviceIndex;
            Age = rawEvent.eventAgeSeconds;
            EventData = rawEvent.data;
        }
    }

    public class OpenVRInputObservable : IInputObservable
    {
        private static readonly ImmutableHashSet<EVREventType> ControllerEvents = ImmutableHashSet.Create(
            EVREventType.VREvent_ButtonPress, EVREventType.VREvent_ButtonUnpress, EVREventType.VREvent_ButtonTouch, EVREventType.VREvent_ButtonUntouch);

        private readonly CVRSystem hmd;

        public IObservable<IInput> InputEvents { get; }

        public OpenVRInputObservable(CVRSystem hmd)
        {
            this.hmd = hmd;

            var eventLauncher = new OpenVREventLauncher(hmd);

            InputEvents =
                from evt in Observable.FromEventPattern<OpenVREventArgs>(eventLauncher, nameof(eventLauncher.OpenVREvent))
                let evtType = evt.EventArgs.EventType
                let data = evt.EventArgs.EventData
                where ControllerEvents.Contains(evtType)
                where evtType == EVREventType.VREvent_ButtonPress || evtType == EVREventType.VREvent_ButtonUnpress
                select new OpenVRControllerInput(hmd, evt.EventArgs.DeviceIndex, evtType, data.controller);            
        }

        private class OpenVRControllerInput : IInput
        {
            public InputType Input { get; }
            public InputState State { get; }
            public double Weight { get; } = 1.0;

            public OpenVRControllerInput(CVRSystem hmd, int index, EVREventType eventType, VREvent_Controller_t contEvent)
            {
                State = eventType == EVREventType.VREvent_ButtonPress ? InputState.Start : InputState.Finish;

                // CONTROLLER KEY BINDINGS ARE HERE
                switch ((EVRButtonId)contEvent.button)
                {
                    case EVRButtonId.k_EButton_System:
                        // used by system, dont handle
                        break;
                    case EVRButtonId.k_EButton_ApplicationMenu:
                        Input = InputType.Pause;
                        break;
                    case EVRButtonId.k_EButton_Grip:
                        break;
                    case EVRButtonId.k_EButton_DPad_Left:
                        break;
                    case EVRButtonId.k_EButton_DPad_Up:
                        break;
                    case EVRButtonId.k_EButton_DPad_Right:
                        break;
                    case EVRButtonId.k_EButton_DPad_Down:
                        break;
                    case EVRButtonId.k_EButton_A:
                        break;
                    case EVRButtonId.k_EButton_Axis2:
                        break;
                    case EVRButtonId.k_EButton_Axis3:
                        break;
                    case EVRButtonId.k_EButton_Axis4:
                        break;
                    case EVRButtonId.k_EButton_SteamVR_Touchpad:
                        Input = hmd.ControllerState(index).rAxis0.y > 0 ? InputType.SpeedUp : InputType.SlowDown;
                        break;
                    case EVRButtonId.k_EButton_SteamVR_Trigger:
                        Input = InputType.PrimarySelect;
                        break;
                    case EVRButtonId.k_EButton_Max:
                        // should never fire
                        break;
                    default:
                        break;
                }
            }


        }

        // polls for OpenVR events and converts them into the .NET event framework
        private class OpenVREventLauncher
        {          
            private const int PollsPerSecond = 90;
            private const int PollIntervalMilis = 1000 / PollsPerSecond;

            private readonly CVRSystem hmd;
            private readonly Timer timer; // need to keep a reference to prevent it from being garbadge collected

            public event EventHandler<OpenVREventArgs> OpenVREvent;

            public OpenVREventLauncher(CVRSystem hmd)
            {
                this.hmd = hmd;

                timer = new Timer(PollEvents, null, 0, PollIntervalMilis); // automatically starts the timer
            }

            private void PollEvents(object stateIgnored)
            {
                VREvent_t vrEvent = new VREvent_t();
                while (hmd.PollNextEvent(ref vrEvent, (uint)Marshal.SizeOf<VREvent_t>()))
                {
                    OpenVREvent?.Invoke(this, new OpenVREventArgs(vrEvent));
                }
            }
        }
    }


}
