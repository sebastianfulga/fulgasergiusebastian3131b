using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Lab_4
{
    public class Triangle3D
    {
        private Vector3 pointA;
        private Vector3 pointB;
        private Vector3 pointC;
        private Color color;
        private bool visibility;
        private float lineWidth;

        // referinta privata 
        private Randomizer localRandomizer;

        public Triangle3D(Randomizer r)
        {
            /*
            pointA = new Vector3(0, 0, 0);
            pointB = new Vector3(0, 0, 0);
            pointC = new Vector3(0, 0, 0);
            */

            pointA = r.Generate3DPoint();
            pointB = r.Generate3DPoint();
            pointC = r.Generate3DPoint();

            color = r.RandomColor();
            visibility = true;
            lineWidth = 1.0f;

            localRandomizer = r;
        }

        public void Draw()
        {
            if (visibility)
            {
                GL.LineWidth(lineWidth);
                // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Begin(PrimitiveType.Triangles);

                GL.Color3(color);
                GL.Vertex3(pointA);

                // GL.Color3(color);
                GL.Vertex3(pointB);

                // GL.Color3(color);
                GL.Vertex3(pointC);

                GL.End();
            } 
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void DiscoMode()
        {
            color = localRandomizer.RandomColor();
        }

        public void Morph()
        {
            int select = localRandomizer.GeneratePositiveInt(3);
            Vector3 tmp = localRandomizer.Generate3DPoint();

            if (select == 0)
            {
                pointA = tmp;
            }
            else
            {
                pointB = tmp;
            }

            if (select == 2)
            {
                pointC = tmp;
            }
        }

        public void Morph2()
        {
            Vector3 tmp = localRandomizer.Generate3DPoint(5);

            pointC = tmp;
        }
    }
}
