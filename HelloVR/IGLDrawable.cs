using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloVR
{
    public interface IVRDrawable
    {
        void Draw(Matrix4 viewProjmatrix, Vector3 camPos);
    }

    public sealed class BasicDrawable : IVRDrawable
    {
        public Action<Matrix4, Vector3> DrawAction { get; }

        public BasicDrawable(Action<Matrix4, Vector3> drawAction)
        {
            DrawAction = drawAction;
        }

        public void Draw(Matrix4 viewProjmatrix, Vector3 camPos)
        {
            DrawAction(viewProjmatrix, camPos);
        }
    }
}
