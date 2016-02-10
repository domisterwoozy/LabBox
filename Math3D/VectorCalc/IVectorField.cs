namespace Math3D.VectorCalc
{
    public interface IVectorField
    {
        /// <summary>
        /// That value of the VectorField at a certain global position.
        /// </summary>
        Vector3 Value(Vector3 pos);

        /// <summary>
        /// Evalutes the curl of this vector field at a certain global position.
        /// </summary>
        Vector3 Curl(Vector3 pos);

        /// <summary>
        /// Evaluates the divergence of this vector field at a certain global position.
        /// </summary>
        double Divergence(Vector3 pos);
    }
}
