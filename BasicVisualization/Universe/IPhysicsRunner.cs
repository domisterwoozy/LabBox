using Physics3D.Universes;

namespace LabBox.Visualization.Universe
{
    public interface IPhysicsRunner
    {
        bool IsPaused { get; }
        IUniverse Universe { get; }

        void PausePhysics();
        void ResumePhysics();
        void StartPhysics();
        void TogglePause();
    }
}