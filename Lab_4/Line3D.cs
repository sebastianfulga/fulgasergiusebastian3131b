using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Lab_4
{
    public class Line3D
    {
        private Vector3 pointA;
        private Vector3 pointB;
        private bool visibility;
        private float width;
        private Color color;
        private float BIG_SIZE = 5.0f;
        private float DEFAULT_SIZE = 1.0f;

        public Line3D(Randomizer _r)
        {
            pointA = new Vector3(0, 0, 0);
            pointB = new Vector3(10, 0, 0);
            visibility = true;
            width = DEFAULT_SIZE;
            color = _r.RandomColor();
        }

        public Line3D(string numeFisier)
        {
            
        }

        public void Draw()
        {
            if (visibility)
            {
                GL.LineWidth(width);

                GL.Begin(PrimitiveType.Lines);

                GL.Color3(color);
                GL.Vertex3(pointA);
                GL.Vertex3(pointB);

                GL.End();
            }
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void ToggleWidth()
        {
            if (width == DEFAULT_SIZE)
            {
                width = BIG_SIZE;
            }
            else
            {
                width = DEFAULT_SIZE;
            }
        }

        public void DiscoMode(Randomizer _r)
        {
            color = _r.RandomColor();
        }
    }
}
