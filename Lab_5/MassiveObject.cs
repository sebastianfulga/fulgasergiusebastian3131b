using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Lab_5
{
    /// <summary>
    ///  laborator 5 - punctul 3 - utilizare fisiere low-poly de tip OBJ 
    /// </summary>
    public class MassiveObject
    {
        private const String FILENAME = @"./../../LowPolyObjFiles/stea2.obj";
        // private const String FILENAME = @"./../../LowPolyObjFiles/soccer_ball.obj";
        // private const String FILENAME = @"./../../LowPolyObjFiles/king_crown.obj";
        // private const String FILENAME = @"./../../LowPolyObjFiles/thumbs.obj";
        private const float FACTOR_SCALARE_IMPORT = 10f;

        private List<Vector3> coordsList;
        private bool visibility;
        private Color meshColor;
        private bool hasError;
        private bool isGravityBound;

        private Randomizer random;

        private const int GRAVITY_OFFSET = 2;

        /// <summary>
        ///  laborator 5 - punctul 3 - citire din fisier 
        /// </summary>
        /// <param name="gravityStatus"></param>
        public MassiveObject(bool gravityStatus)
        {
            try
            {
                random = new Randomizer();

                coordsList = LoadFromObjFile(FILENAME);

                if (coordsList.Count == 0)
                {
                    Console.WriteLine("Crearea obiectului a esuat: obiect negasit/coordonate lipsa!");
                    return;
                }

                // creare obiecte cu un offset de dimensiune / inaltime / directia OX - OZ variabila
                int size_offset = random.RandomInt(1, 2);
                int height_offset = random.RandomInt(40, 75);
                int radial_offset = random.RandomInt(-150, 150);
                int rad_offset = random.RandomInt(-150, 150);

                for (int i = 0; i < coordsList.Count; i++)
                {
                    coordsList[i] = new Vector3(coordsList[i].X * size_offset + radial_offset,
                        coordsList[i].Y * size_offset + height_offset,
                        coordsList[i].Z * size_offset + rad_offset);
                }

                visibility = true;
                meshColor = random.RandomColor();
                hasError = false;
                isGravityBound = gravityStatus;
                // Console.WriteLine("Obiect 3D încarcat - " + coordsList.Count.ToString() + " vertexuri disponibile!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: assets file <" + FILENAME + "> is missing!!!");
                hasError = true;

                Console.WriteLine(ex.Message);
            }
        }

        public void ToggleVisibility()
        {
            if (hasError == false)
            {
                visibility = !visibility;
            }
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

        public void Draw()
        {
            if (hasError == false && visibility == true)
            {
                GL.Color3(meshColor);
                GL.Begin(PrimitiveType.Triangles);
                foreach (var vert in coordsList)
                {
                    GL.Vertex3(vert);
                }
                GL.End();
            }
        }

        private List<Vector3> LoadFromObjFile(string fname)
        {
            List<Vector3> vlc3 = new List<Vector3>();

            var lines = File.ReadLines(fname);
            foreach (var line in lines)
            {
                if (line.Trim().Length > 2)
                {
                    string ch1 = line.Trim().Substring(0, 1);
                    string ch2 = line.Trim().Substring(1, 1);
                    if (ch1 == "v" && ch2 == " ")
                    {

                        string[] block = line.Trim().Split(' ');
                        if (block.Length == 4)
                        {
                            // ATENTIE: Pericol!!!
                            float xval = float.Parse(block[1].Trim()) * FACTOR_SCALARE_IMPORT;
                            float yval = float.Parse(block[2].Trim()) * FACTOR_SCALARE_IMPORT;
                            float zval = float.Parse(block[3].Trim()) * FACTOR_SCALARE_IMPORT;

                            vlc3.Add(new Vector3((int)xval, (int)yval, (int)zval));

                        }
                    }
                }
            }

            return vlc3;
        }

        public bool GroundCollisionDetected()
        {
            foreach (Vector3 v in coordsList)
            {
                if (v.Y <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdatePosition(bool gravityStatus)
        {
            if (visibility && gravityStatus && !GroundCollisionDetected())
            {
                for (int i = 0; i < coordsList.Count; i++)
                {
                    coordsList[i] = new Vector3(coordsList[i].X, coordsList[i].Y - GRAVITY_OFFSET, coordsList[i].Z);
                }
            }
        }
    }
}
