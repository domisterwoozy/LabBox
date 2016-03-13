using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization
{
    public static class VisUtil
    {
        private static readonly Random rand = new Random();

        public static Color RandomOpaqueColor() => Color.FromArgb(255, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        public static Color RandomColor() => Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        public static Color RandomColor(int transparency) => Color.FromArgb(transparency, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));

        public static IntPtr GetBits(this Bitmap bm)
        {
            var bmd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            return bmd.Scan0;
        }

    }
}
