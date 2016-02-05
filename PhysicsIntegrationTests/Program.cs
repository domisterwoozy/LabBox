using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsIntegrationTests
{
    class Program
    {
        const string filepath = @"D:\Development\VisualC#\LabBox\IntegratedResults";

        static void Main(string[] args)
        {
            string output = CircleOrbitTest.RunTest();
            string fileName = filepath + "\\" + CircleOrbitTest.Name + "-" + DateTime.Now.ToString().Replace(':', '-').Replace('\\', '-').Replace('/', '-') + ".csv";
            File.WriteAllText(fileName, output);
        }
    }
}
