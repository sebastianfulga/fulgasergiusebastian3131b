using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Immediate_Mode
{
    class Camera3D
    {
        private Vector3 eye = new Vector3(100, 150, 50);
        private Vector3 target = new Vector3(0, 25, 0);
        private Vector3 up = new Vector3(0, 1, 0);
        private const int MOVEMENT_UNIT = 2;

        public void SetCamera()
        {
            Matrix4 camera = Matrix4.LookAt(eye, target, up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref camera);
        }

        public void RotateRight()
        {
            eye = new Vector3(eye.X + MOVEMENT_UNIT, eye.Y + MOVEMENT_UNIT, eye.Z - MOVEMENT_UNIT);
            SetCamera();
        }

        public void RotateLeft()
        {
            eye = new Vector3(eye.X - MOVEMENT_UNIT, eye.Y - MOVEMENT_UNIT, eye.Z + MOVEMENT_UNIT);
            SetCamera();
        }

        public void RotateUp()
        {
            eye = new Vector3(eye.X, eye.Y + 5, eye.Z);
            SetCamera();
        }

        public void RotateDown()
        {
            eye = new Vector3(eye.X, eye.Y - 5, eye.Z);
            SetCamera();
        }
    }
}
