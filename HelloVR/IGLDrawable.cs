using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloVR
{
    public interface IGLDrawable
    {
        void Update();
        void Draw(Matrix4 viewProjmatrix);
    }

    public sealed class BasicDrawable : IGLDrawable
    {
        public Action UpdateAction { get; }
        public Action<Matrix4> DrawAction { get; }

        public BasicDrawable(Action updateAction, Action<Matrix4> drawAction)
        {
            UpdateAction = updateAction;
            DrawAction = drawAction;
        }

        public void Draw(Matrix4 viewProjmatrix)
        {
            DrawAction(viewProjmatrix);
        }

        public void Update()
        {
            UpdateAction();
        }
    }
}
