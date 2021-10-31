using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Lab_4
{
    public class Camera3D
    {
        private Vector3 eye = new Vector3(30, 30, 30);
        private Vector3 target = new Vector3(0, 0, 0);
        private Vector3 up = new Vector3(0, 1, 0);
        private const int MOVEMENT_UNIT = 1;

        /// <summary>
        /// setare camera
        /// </summary>
        public void SetCamera()
        {
            Matrix4 camera = Matrix4.LookAt(eye, target, up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref camera);
        }

        /// <summary>
        /// rotire la dreapta
        /// </summary>
        public void RotateRight()
        {
            eye = new Vector3(eye.X + MOVEMENT_UNIT, eye.Y + MOVEMENT_UNIT, eye.Z - MOVEMENT_UNIT);
            SetCamera();
        }

        /// <summary>
        /// rotire la stanga
        /// </summary>
        public void RotateLeft()
        {
            eye = new Vector3(eye.X - MOVEMENT_UNIT, eye.Y - MOVEMENT_UNIT, eye.Z + MOVEMENT_UNIT);
            SetCamera();
        }

        /// <summary>
        /// rotire sus
        /// </summary>
        public void RotateUp()
        {
            eye = new Vector3(eye.X, eye.Y + 2, eye.Z);
            SetCamera();
        }

        /// <summary>
        /// rotire jos
        /// </summary>
        public void RotateDown()
        {
            eye = new Vector3(eye.X, eye.Y - 2, eye.Z);
            SetCamera();
        }
    }
}
