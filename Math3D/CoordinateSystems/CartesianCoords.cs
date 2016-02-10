namespace Math3D.CoordinateSystems
{
    public class CartesianCoords : ICoordinateSystem<CartesianCoords>
    {
        public static readonly CartesianCoords System = new CartesianCoords();
        private CartesianCoords() { }

        public Coords3D<CartesianCoords> FromCartesian(Vector3 cartCoords) => new Coords3D<CartesianCoords>(this, cartCoords.X, cartCoords.Y, cartCoords.Z);
        public Vector3 ToCartesian(Coords3D<CartesianCoords> coords) => new Vector3(coords.FirstComponent, coords.SecondComponent, coords.ThirdComponent);
        public double Jacobian(Coords3D<CartesianCoords> coords) => 1;
        public Matrix3 Metric(Coords3D<CartesianCoords> coords) => Matrix3.Identity;
        public Vector3 FirstUnitVector(Coords3D<CartesianCoords> coords) => Vector3.I;
        public Vector3 SecondUnitVector(Coords3D<CartesianCoords> coords) => Vector3.J;
        public Vector3 ThirdUnitVector(Coords3D<CartesianCoords> coords) => Vector3.K;
    }
}
