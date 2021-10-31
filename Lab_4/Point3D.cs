using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Lab_4
{
    public class Point3D
    {
        private Vector3 position;
        private bool visibility;
        private Color color;
        private float size;

        public Point3D(Randomizer _r)
        {
            position = new Vector3(0, 0, 0);
            visibility = true;
            color = _r.RandomColor();
            size = 5.0f;
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void DrawPoint3D()
        {
            if (visibility)
            {
                GL.PointSize(size);
                GL.Begin(PrimitiveType.Points);
                GL.Color3(color);
                GL.Vertex3(position);
                GL.End();
            }
        }

        public void ResizePoint3D()
        {
            if (size == 5.0f)
            {
                size = 15.0f;
            }
            else
            {
                size = 5.0f;
            }
        }
    }
}
