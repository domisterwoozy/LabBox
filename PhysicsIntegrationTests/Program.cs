using System;
using System.IO;

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
