using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    public interface ICamera
    {
        bool IsLocked { get; set; }

        // camera focal properties
        float VertFOV { get; }
        float MaxRange { get; }
        float MinRange { get; }
        float AspectRatio { get; }

        // current camera state
        Vector3 Pos { get; } 
        Vector3 UpDir { get; }
        Vector3 LookAtPos { get; }  
    }

    
}
