using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;

namespace Lab_5
{
    /// <summary>
    /// Obiectul acesta va fi plasat in mediul 3D
    /// sub influenta "gravitatiei" va cadea in jos
    /// pana atinge solul gridului 
    /// </summary>
    public class Objectoid
    {
        private bool visibility;
        private bool isGravityBound;
        private Color color;
        private List<Vector3> listaCoordonate;
        private Randomizer random;

        private const int GRAVITY_OFFSET = 1;

        /// <summary>
        /// constructor implicit 
        /// vor avea loc initializarile 
        /// </summary>
        public Objectoid(bool gravityStatus, List<Vector3> vertexuri)
        {
            random = new Randomizer();
            visibility = true;
            isGravityBound = gravityStatus;
            color = random.RandomColor();

            listaCoordonate = new List<Vector3>();

            // creare obiecte cu un offset de dimensiune / inaltime / directia OX - OZ variabila
            int size_offset = random.RandomInt(3, 7);
            int height_offset = random.RandomInt(40, 75);
            int radial_offset = random.RandomInt(-40, 40);
            int rad_offset = random.RandomInt(-40, 40);

            for (int i = 0; i < 10; i++)
            {
                listaCoordonate.Add(
                    new Vector3(vertexuri[i].X * size_offset + radial_offset, 
                    vertexuri[i].Y * size_offset + height_offset, 
                    vertexuri[i].Z * size_offset + rad_offset));
            }
        }

        public void Draw()
        {
            if (visibility)
            {
                GL.Color3(color);
                GL.Begin(PrimitiveType.QuadStrip);

                foreach (Vector3 v in listaCoordonate)
                {
                    GL.Vertex3(v);
                }
                GL.End();
            }
        }

        public void UpdatePosition(bool gravityStatus)
        {
            if (visibility && gravityStatus && !GroundCollisionDetected())
            {
                for (int i = 0; i < listaCoordonate.Count; i++)
                {
                    listaCoordonate[i] = new Vector3(listaCoordonate[i].X, listaCoordonate[i].Y - GRAVITY_OFFSET, listaCoordonate[i].Z);
                }
            }
        }

        public bool GroundCollisionDetected()
        {
            foreach (Vector3 v in listaCoordonate)
            {
                if (v.Y <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void ToggleGravity()
        {
            isGravityBound = !isGravityBound;
        }

        public void SetGravity()
        {
            isGravityBound = true;
        }

        public void DisableGravity()
        {
            isGravityBound = false;
        }
    }
}
