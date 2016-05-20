using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;

namespace HelloVR
{
    class Program
    {      
        public static void Main(string[] args)
        {
            var window = new HelloVRWindow("Hello VR");
            window.Run();

        }      
    }
}
