using System;

namespace Math3D.CoordinateSystems
{
    /// <summary>
    /// An arbitrary container for coordinates in 3 dimensions.
    /// </summary>
    public struct Coords3D<T> : IEquatable<Coords3D<T>> where T : ICoordinateSystem<T>
    {
        public double FirstComponent { get; }
        public double SecondComponent { get; }
        public double ThirdComponent { get; }
        public T System { get; }

        public Coords3D(T system, double first, double second, double third)
        {
            System = system;
            FirstComponent = first;
            SecondComponent = second;
            ThirdComponent = third;
        }

        #region Equality
        public bool Equals(Coords3D<T> v)
        {
            if (FirstComponent != v.FirstComponent) return false;
            if (SecondComponent != v.SecondComponent) return false;
            if (ThirdComponent != v.ThirdComponent) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + FirstComponent.GetHashCode();
            result = 31 * result + SecondComponent.GetHashCode();
            result = 31 * result + ThirdComponent.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Coords3D<T> v;
            if (obj is Coords3D<T>) v = (Coords3D<T>)obj;
            else return false;
            return Equals(v);
        }

        public static bool operator ==(Coords3D<T> a, Coords3D<T> b) => a.Equals(b);
        public static bool operator !=(Coords3D<T> a, Coords3D<T> b) => !(a == b);
        #endregion
    }

    /// <summary>
    /// A custom 3 dimensional coordinate system.
    /// </summary>
    public interface ICoordinateSystem<T> where T : ICoordinateSystem<T>
    {
        /// <summary>
        /// Coordinate transformation from the custom system to cartesian.
        /// </summary>
        Vector3 ToCartesian(Coords3D<T> coords);

        /// <summary>
        /// Coordinate transformation from catesian to the custom coordinate system.
        /// </summary>
        Coords3D<T> FromCartesian(Vector3 cartCoords);

        /// <summary>
        /// The unit vector of the first coordinate in cartesian coordinates.
        /// </summary>
        Vector3 FirstUnitVector(Coords3D<T> coords);
        /// <summary>
        /// The unit vector of the second coordinate in cartesian coordinates.
        /// </summary>
        Vector3 SecondUnitVector(Coords3D<T> coords);
        /// <summary>
        /// The unit vector of the third coordinate in cartesian coordinates.
        /// </summary>
        Vector3 ThirdUnitVector(Coords3D<T> coords);

        Matrix3 Metric(Coords3D<T> coords);
        double Jacobian(Coords3D<T> coords);
    }

    /// <summary>
    /// Coordinate System extension methods
    /// </summary>
    public static class CoordinateSystems
    {
        /// <summary>
        /// Converts a vector in an arbitrary coordinate system to an equivalent cartesian vector. 
        /// </summary>
        public static Vector3 ToCartesianVector<T>(this Coords3D<T> arbitraryVectorDirection, Coords3D<T> vectorOrigin) where T : ICoordinateSystem<T>
        {
            return arbitraryVectorDirection.FirstComponent * arbitraryVectorDirection.System.FirstUnitVector(vectorOrigin) +
                arbitraryVectorDirection.SecondComponent * arbitraryVectorDirection.System.SecondUnitVector(vectorOrigin) +
                arbitraryVectorDirection.ThirdComponent * arbitraryVectorDirection.System.ThirdUnitVector(vectorOrigin);            
        }
    }
}
