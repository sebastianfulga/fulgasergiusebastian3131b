using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lab_5
{
    public class Camera3DIsometric
    {
        private Vector3 eye;
        private Vector3 target;
        private Vector3 up_vector;

        private const int MOVEMENT_UNIT = 1;
        private const int ZOOM_UNIT = 4;
        private const int FAR_UNIT = 4; 

        public Camera3DIsometric()
        {
            eye = new Vector3(200, 175, 25);
            target = new Vector3(0, 25, 0);
            up_vector = new Vector3(0, 1, 0);
        }

        public Camera3DIsometric(int _eyeX, int _eyeY, int _eyeZ, int _targetX, int _targetY, int _targetZ, int _upX, int _upY, int _upZ)
        {
            eye = new Vector3(_eyeX, _eyeY, _eyeZ);
            target = new Vector3(_targetX, _targetY, _targetZ);
            up_vector = new Vector3(_upX, _upY, _upZ);
        }

        public Camera3DIsometric(Vector3 _eye, Vector3 _target, Vector3 _up)
        {
            eye = _eye;
            target = _target;
            up_vector = _up;
        }

        public void SetCamera()
        {
            Matrix4 camera = Matrix4.LookAt(eye, target, up_vector);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref camera);
        }

        public void MoveRight()
        {
            eye = new Vector3(eye.X, eye.Y, eye.Z - MOVEMENT_UNIT);
            target = new Vector3(target.X, target.Y, target.Z - MOVEMENT_UNIT);
            SetCamera();
        }
        public void MoveLeft()
        {
            eye = new Vector3(eye.X, eye.Y, eye.Z + MOVEMENT_UNIT);
            target = new Vector3(target.X, target.Y, target.Z + MOVEMENT_UNIT);
            SetCamera();
        }

        public void MoveForward()
        {
            eye = new Vector3(eye.X - MOVEMENT_UNIT, eye.Y, eye.Z);
            target = new Vector3(target.X - MOVEMENT_UNIT, target.Y, target.Z);
            SetCamera();
        }

        public void MoveBackward()
        {
            eye = new Vector3(eye.X + MOVEMENT_UNIT, eye.Y, eye.Z);
            target = new Vector3(target.X + MOVEMENT_UNIT, target.Y, target.Z);
            SetCamera();
        }

        public void MoveUp()
        {
            eye = new Vector3(eye.X, eye.Y + MOVEMENT_UNIT, eye.Z);
            target = new Vector3(target.X, target.Y + MOVEMENT_UNIT, target.Z);
            SetCamera();
        }

        public void MoveDown()
        {
            eye = new Vector3(eye.X, eye.Y - MOVEMENT_UNIT, eye.Z);
            target = new Vector3(target.X, target.Y - MOVEMENT_UNIT, target.Z);
            SetCamera();
        }

        public void Far()
        {
            eye = new Vector3(eye.X + ZOOM_UNIT, eye.Y + ZOOM_UNIT, eye.Z);
            target = new Vector3(target.X + ZOOM_UNIT, target.Y + ZOOM_UNIT, target.Z);
            SetCamera();
        }

        public void Zoom()
        {
            eye = new Vector3(eye.X - ZOOM_UNIT, eye.Y - ZOOM_UNIT, eye.Z);
            target = new Vector3(target.X - ZOOM_UNIT, target.Y - ZOOM_UNIT, target.Z);
            SetCamera();
        }

        public void Nearly()
        {
            eye = new Vector3(125, 100, 25);
            target = new Vector3(0, 25, 0);
            SetCamera();
        }

        public void FarAway()
        {
            eye = new Vector3(200, 175, 25);
            target = new Vector3(0, 25, 0);
            SetCamera();
        }
    }
}
