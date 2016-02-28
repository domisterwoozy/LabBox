using Math3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    public struct Vertex
    {
        private static readonly Random rand = new Random();

        public Vector3 Pos { get; }
        public Vector3 Normal { get; }
        public Color Color { get; }

        public Vertex(Vector3 pos, Color color, Vector3 normal)
        {
            Pos = pos;
            Color = color;
            Normal = normal;
        }

        public Vertex(double x, double y, double z)
        {
            Pos = new Vector3(x, y, z);
            Color = Color.FromArgb(255, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
            Normal = Pos.UnitDirection;
        }

        public Vertex(double x, double y, double z, Color color)
        {
            Pos = new Vector3(x, y, z);
            Color = color;
            Normal = Pos.UnitDirection;
        }

        public Vertex(double x, double y, double z, Color color, Vector3 normal)
        {
            Pos = new Vector3(x, y, z);
            Color = color;
            Normal = normal;
        }
    }
}
