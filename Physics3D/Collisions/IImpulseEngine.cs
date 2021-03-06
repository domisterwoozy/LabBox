﻿using Math3D;
using Math3D.Geometry;
using Physics3D.Bodies;
using Physics3D.Kinematics;
using Physics3D.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Collisions
{  
    public interface IImpulseEngine
    {
        double Epsilon { get; set; }
        /// <summary>
        /// Determines the impulse generated by the contact between two bodies.        
        /// </summary>
        Vector3 Collide(Contact c);
    }
    
}
