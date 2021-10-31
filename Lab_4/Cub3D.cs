using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Lab_4
{
    public class Cub3D
    {
        /// <summary>
        /// declarare constante
        /// </summary>
        private const int NUMAR_CULORI = 6;
        /// <summary>
        /// declarare lista de tip Vector3 - vertexuri
        /// declarare lista de tip Color - culori
        /// declarare tip de data de tip bool - visibility
        /// declarare tip de data de tip float - lineWidth
        /// </summary>
        private List<Vector3> vertexuri;
        private List<Color> culori;
        private bool visibility;
        private float lineWidth;

        // referinta privata 
        private Randomizer localRandomizer;

        /// <summary>
        /// constructor cu parametri ce primeste un obiect de tip Randomizer si numele fisierului
        /// se vor citi coordonatele cubului din fisierul text, 
        /// apoi se va crea un vertex cu fiecare linie a fisierului, vertexul fiind adaugat in lista de vertexuri 
        /// in total avem 6 fete, deci 12 triughiuri a cate 3 vertexuri fiecare, deci vom avea 36 de vertexuri
        /// se va seta variabila bool visibility la true si lineWidth la valoarea 1.0f
        /// </summary>
        /// <param name="r">Randomizer</param>
        /// <param name="numeFisier">numele fisierului text</param>
        public Cub3D(Randomizer r, string numeFisier)
        {
            vertexuri = new List<Vector3>();

            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    var numere = linie.Split(',');
                    int i = 0;
                    float[] coordonate = new float[3];
                    foreach (var nr in numere)
                    {
                        coordonate[i++] = float.Parse(nr);
                    }
                    vertexuri.Add(new Vector3(coordonate[0], coordonate[1], coordonate[2]));
                }
            }

            culori = new List<Color>();

            for (int i = 0; i < NUMAR_CULORI; i++)
            {
                culori.Add(r.RandomColor());
            }

            visibility = true;
            lineWidth = 10.0f;

            localRandomizer = r;
        }

        /// <summary>
        /// laborator 4 - punctul 2
        /// se modifica valorile RGB pentru fiecare vertex, culoarea fiind generata 
        /// </summary>
        public void Draw()
        {
            if (visibility)
            {
                GL.LineWidth(lineWidth);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Begin(PrimitiveType.Triangles);
                for (int i = 0; i < 35; i = i + 3)
                {
                    GL.Color3(culori[i/6]);
                    // GL.Color4(culori[i / 6]);
                    GL.Vertex3(vertexuri[i]);
                    GL.Vertex3(vertexuri[i+1]);
                    GL.Vertex3(vertexuri[i+2]);
                }
                GL.End();
            }
        }

        public void SchimbareCuloareFata1()
        {
            Color col = localRandomizer.RandomColor();
            culori[0] = col;
        }

        public void SchimbareCuloareFata2()
        {
            Color col = localRandomizer.RandomColor();
            culori[1] = col;
        }

        public void SchimbareCuloareFata3()
        {
            Color col = localRandomizer.RandomColor();
            culori[2] = col;
        }

        public void SchimbareCuloareFata4()
        {
            Color col = localRandomizer.RandomColor();
            culori[3] = col;
        }

        public void SchimbareCuloareFata5()
        {
            Color col = localRandomizer.RandomColor();
            culori[4] = col;
        }

        public void SchimbareCuloareFata6()
        {
            Color col = localRandomizer.RandomColor();
            culori[5] = col;
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        public void AfisareRBGValues()
        {
            Console.WriteLine("\n\tValori RGB");
            for (int i = 0; i < NUMAR_CULORI; i++)
            {
                Console.WriteLine($"\tCuloare [ {0} ]: Tip de data Color: { culori[i]}");
            }
        }

        public void DiscoMode()
        {
            List<Color> culoriRandomizer = new List<Color>();
            for (int i = 0; i < NUMAR_CULORI; i++)
            {
                culoriRandomizer.Add(localRandomizer.RandomColor());
            }

            culori = culoriRandomizer;
        }

        public void DrawVertexRGB()
        {
            if (visibility)
            {
                GL.LineWidth(lineWidth);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Begin(PrimitiveType.Triangles);
                for (int i = 0; i < 35; i = i + 3)
                {
                    GL.Color3(localRandomizer.RandomColor());
                    GL.Vertex3(vertexuri[i]);

                    GL.Color3(localRandomizer.RandomColor());
                    GL.Vertex3(vertexuri[i + 1]);

                    GL.Color3(localRandomizer.RandomColor());
                    GL.Vertex3(vertexuri[i + 2]);
                }
                GL.End();
            }
        }
    }
}
