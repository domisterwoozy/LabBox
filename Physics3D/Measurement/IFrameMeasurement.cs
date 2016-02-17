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
        public double FrameTime { get; }

        public FrameReading(T res, double frameLength)
        {
            Result = res;
            FrameTime = frameLength;
        }
    }

    /// <summary>
    /// A measurement that occurs over the course of one frame.
    /// Attach the measurement to the universe using Listen and after the universe updates one physics frame the task will return a result.
    /// </summary>
    public interface IFrameMeasurement<T>
    {
        event EventHandler<FrameReading<T>> ReadingComplete;
        string Name { get; }        
        void Listen(IUniverse uni);
    }    
}
