using Physics3D.Bodies;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Measurement
{
    public class FrameReading<T> : EventArgs
    {
        public T Result { get; }
        public double FrameLength { get; }

        public FrameReading(T res, double frameLength)
        {
            Result = res;
            FrameLength = frameLength;
        }
    }

    /// <summary>
    /// A measurement on a universe that occurs over the course of one frame.
    /// Attach the measurement to the universe using Listen and after the universe updates one physics the event will fire with the result
    /// </summary>
    public interface IFrameMeasurement<T>
    {
        event EventHandler<FrameReading<T>> ReadingComplete;
        string Name { get; }        
        void Listen(IUniverse uni);
    }

    /// <summary>
    /// A measurement on a body that occurs over the course of one frame.
    /// Attach the measurement to the universe using Listen and after the body updates one physics frame the event will fire with the result.
    /// </summary>
    public interface IFrameBodyMeasurement<T>
    {
        event EventHandler<FrameReading<T>> ReadingComplete;
        string Name { get; }
        void Listen(IBody uni);
    }
}
