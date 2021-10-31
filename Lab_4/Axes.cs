using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Lab_4
{
    public class Axes
    {
        private const int XYZ_SIZE = 75;
        private bool visibility;
        private float lineWidth;

        public Axes()
        {
            visibility = true;
            lineWidth = 1.0f;
        }

        public void DrawAxes()
        {
            if (visibility)
            {
                GL.LineWidth(lineWidth);

                // Desenează axa Ox (cu roșu).
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Red);
                GL.Vertex3(5, 5, 5);
                GL.Vertex3(XYZ_SIZE, 5, 5);

                // Desenează axa Oy (cu galben).
                GL.Color3(Color.Yellow);
                GL.Vertex3(5, 5, 5);
                GL.Vertex3(5, XYZ_SIZE, 5);

                // Desenează axa Oz (cu verde).
                GL.Color3(Color.Green);
                GL.Vertex3(5, 5, 5);
                GL.Vertex3(5, 5, XYZ_SIZE);

                GL.End();
            }
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }
    }
}
