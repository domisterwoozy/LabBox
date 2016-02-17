using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Measurement
{
    public abstract class FrameProcessor<T> : IFrameMeasurement<T>
    {
        public event EventHandler<FrameReading<T>> ReadingComplete;

        public string Name { get; }

        public FrameProcessor(string name)
        {
            Name = name;
        }

        public void Listen(IUniverse uni)
        {
            ProcessBeforeFrame(uni);
            uni.FrameFinished += (sender, e) =>
            {
                T res = ProcessAfterFrame(uni);
                ReadingComplete?.Invoke(this, new FrameReading<T>(res, e.FrameLength));
            };
        }

        protected abstract void ProcessBeforeFrame(IUniverse uni);
        protected abstract T ProcessAfterFrame(IUniverse uni);
    }
}
