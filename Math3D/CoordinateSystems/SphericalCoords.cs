using Math3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.CoordinateSystems
{
    /// <summary>
    /// The spherical coordinate system (r, theta, phi)
    /// x = r cos(theta) sin(phi)
    /// y = r sin(theta) sin(phi)
    /// z = r cos(phi)
    /// using the "physics version" here http://mathworld.wolfram.com/SphericalCoordinates.html
    /// </summary>
    public class SphericalCoords : ICoordinateSystem<SphericalCoords>
    {
        public static readonly SphericalCoords System = new SphericalCoords();
        private SphericalCoords() { }

        public Coords3D<SphericalCoords> FromCartesian(Vector3 cartCoords)
        {
            double r = cartCoords.Magnitude;
            double theta = Math.Atan2(cartCoords.Y, cartCoords.X); 
            double phi = Math.Acos(cartCoords.Z / r);
            return new Coords3D<SphericalCoords>(this, r, theta, phi);
        }      

        public Vector3 ToCartesian(Coords3D<SphericalCoords> spherCoords)
        {
            double x = spherCoords.FirstComponent * Math.Cos(spherCoords.SecondComponent) * Math.Sin(spherCoords.ThirdComponent);
            double y = spherCoords.FirstComponent * Math.Sin(spherCoords.SecondComponent) * Math.Sin(spherCoords.ThirdComponent);
            double z = spherCoords.FirstComponent * Math.Cos(spherCoords.ThirdComponent);
            return new Vector3(x, y, z);
        }            

        public Vector3 FirstUnitVector(Coords3D<SphericalCoords> coords)
        {
            return new Vector3(
                Math.Cos(coords.SecondComponent) * Math.Sin(coords.ThirdComponent),
                Math.Sin(coords.SecondComponent) * Math.Sin(coords.ThirdComponent),
                Math.Cos(coords.ThirdComponent)
            );
        }              

        public Vector3 SecondUnitVector(Coords3D<SphericalCoords> coords) => 
            new Vector3(-Math.Sin(coords.SecondComponent), Math.Cos(coords.SecondComponent), 0);       

        public Vector3 ThirdUnitVector(Coords3D<SphericalCoords> coords)
        {
            return new Vector3(
                Math.Cos(coords.SecondComponent) * Math.Cos(coords.ThirdComponent),
                Math.Sin(coords.SecondComponent) * Math.Cos(coords.ThirdComponent),
                -Math.Sin(coords.ThirdComponent)
            );
        }

        public double Jacobian(Coords3D<SphericalCoords> coords) => coords.FirstComponent * coords.FirstComponent * Math.Sin(coords.ThirdComponent);
        public Matrix3 Metric(Coords3D<SphericalCoords> coords) => Matrix3.DiagMatrix(1, Math.Pow(coords.FirstComponent * Math.Sin(coords.ThirdComponent), 2), Math.Pow(coords.FirstComponent, 2));
    }
}
