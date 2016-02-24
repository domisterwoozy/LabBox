using BasicVisualization.Implementations.OpenGL;
using BasicVisualization.Universe.ViewModel;
using Math3D;
using Physics3D.Bodies;
using Physics3D.Forces;
using Physics3D.Universes;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BasicVisualization
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var b = BodyFactory.PointMass(1.0, Vector3.Zero, Vector3.I);
            var b1 = BodyFactory.PointMass(1.0, new Vector3(0, 5, 0), -Vector3.I);
            var uni = new BasicUniverse();
            uni.Bodies.Add(b);
            uni.Bodies.Add(b1);
            uni.ForceFields.Add(ForceFields.Gravity(b, 100.0));

            Task.Run(() => RunPhysics(uni));

            using (ILabBoxVis vis = new BasicVis(new BasicGraphicalBody(b), new BasicGraphicalBody(b1)))
            {
                vis.RunVis();
            }
        }

        private static void RunPhysics(IUniverse uni)
        {
            var sw = new Stopwatch();
            sw.Start();
            TimeSpan lastTime = sw.Elapsed;
            while(true)
            {
                TimeSpan delta = sw.Elapsed - lastTime;
                lastTime = sw.Elapsed;
                uni.Update(delta.TotalSeconds);                
            }       
        }

        static void TimerTest()
        {
            int xcnt = 0;
            long xdelta, xstart;
            xstart = DateTime.UtcNow.Ticks;
            do
            {
                xdelta = DateTime.UtcNow.Ticks - xstart;
                xcnt++;
            } while (xdelta == 0);

            Console.WriteLine("DateTime:\t{0} ms, in {1} cycles", xdelta / (10000.0), xcnt);

            int ycnt = 0, ystart;
            long ydelta;
            ystart = Environment.TickCount;
            do
            {
                ydelta = Environment.TickCount - ystart;
                ycnt++;
            } while (ydelta == 0);

            Console.WriteLine("Environment:\t{0} ms, in {1} cycles ", ydelta, ycnt);


            Stopwatch sw = new Stopwatch();
            int zcnt = 0;
            long zstart, zdelta;

            sw.Start();
            zstart = sw.ElapsedTicks; // This minimizes the difference (opposed to just using 0)
            do
            {
                zdelta = sw.ElapsedTicks - zstart;
                zcnt++;
            } while (zdelta == 0);
            sw.Stop();

            Console.WriteLine("StopWatch:\t{0} ms, in {1} cycles", (zdelta * 1000.0) / Stopwatch.Frequency, zcnt);
            Console.ReadKey();
        }
    }   
}
