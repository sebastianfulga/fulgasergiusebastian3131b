using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Lab_5
{
    public class Window3D : GameWindow
    {
        /// <summary>
        /// declarare axe de coordonate, grid, Camera3D isometrica, Randomizer, 
        /// variabile pentru keyboard state, respectiv mouse state
        /// </summary>
        private KeyboardState previousKeyboard;
        private MouseState previousMouse;
        private readonly Randomizer random;
        private readonly Axes axe;
        private readonly Grid grid;
        private readonly Camera3DIsometric camera;

        /// <summary>
        /// declarare lista de obiecte de tip Objectoid
        /// </summary>
        private List<Objectoid> listaObiecte;

        /// <summary>
        /// declarare variabila de tip bool pentru a simula gravitatea
        /// </summary>
        private bool GRAVITY = true;

        /// <summary>
        /// declarare lista de obiecte de tip Vector3 pentru vertexurile necesare pentru cub 
        /// </summary>
        private List<Vector3> vertexuri;

        /// <summary>
        /// declarare lista de obiecte de tip MassiveObject 
        /// </summary>
        private List<MassiveObject> listaMassiveObj;

        // DEFAULTS 
        private readonly Color DEFAULT_BACKGROUND_COLOR = Color.FromArgb(49, 50, 51);

        public Window3D() : base(1280, 768, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            // initializari
            random = new Randomizer();
            axe = new Axes();
            grid = new Grid();
            camera = new Camera3DIsometric();

            // vertexuri = new List<Vector3>();
            vertexuri = citireVertexuriDinFisier(@"./../../coordonate.txt");

            listaObiecte = new List<Objectoid>();

            listaMassiveObj = new List<MassiveObject>();

            DisplayHelp();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Fastest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // set background
            GL.ClearColor(DEFAULT_BACKGROUND_COLOR);

            // set viewport
            GL.Viewport(0, 0, this.Width, this.Height);

            // set perspective 
            Matrix4 perspectiva = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Width / (float)this.Height, 1, 1024);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiva);

            // set the eye - the camera 
            camera.SetCamera();
        }

        /// <summary>
        /// laborator 5 - punctul 1 si 2 - la apasarea unui click se va genera cate un obiect random 
        /// care se va deplasa in directia jos, iar la contactul cu planul Oxz deplasarea va inceta 
        /// pentru animatie s-a folosit un contor de valoare de update a metodei OnUpdateFrame()
        /// s-a lucrat in maniera POO inclusiv pentru camera
        /// aplicatia manipuleaza valorile camerei prin apasarea tastelor W, A, S, D, E, Q 
        /// de asemenea, aplicatia permite repozitionarea la 2 locatii predefinite, aproape si departe
        /// ESC, H, R, B - optiuni legate de inchiderea aplicatiei, afisare meniu, schimbare culoare de fundal 
        /// V, P, O, U - toggle visibility 
        /// Z - camera zoom 
        /// F - camera far
        /// M - implementare optiune departe
        /// N - implementare optiune aproape
        /// left mouse button - generare obiect nou la o inaltime aleatoare
        /// right mouse button - curata lista de obiecte
        /// space - generare MassiveObj nou la o inaltime aleatoare
        /// X - curata lista de Massive Objects
        /// G - manipuleaza gravitatea
        /// s-au implementat si aspecte de update falling logic / gravity 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            // codul de logica 
            KeyboardState currentKeyboard = Keyboard.GetState();
            MouseState currentMouse = Mouse.GetState();

            if (currentKeyboard[Key.Escape])
            {
                Exit();
            }

            if (currentKeyboard[Key.H] && !previousKeyboard[Key.H])
            {
                DisplayHelp();
            }

            if (currentKeyboard[Key.R] && !previousKeyboard[Key.R])
            {
                GL.ClearColor(DEFAULT_BACKGROUND_COLOR);
            }

            if (currentKeyboard[Key.B] && !previousKeyboard[Key.B])
            {
                GL.ClearColor(random.RandomColor());
            }

            if (currentKeyboard[Key.V] && !previousKeyboard[Key.V])
            {
                grid.ToggleVisibility();
            }

            if (currentKeyboard[Key.P] && !previousKeyboard[Key.P])
            {
                axe.ToggleVisibility();
            }

            if (currentKeyboard[Key.O] && !previousKeyboard[Key.O])
            {
                foreach (Objectoid obj in listaObiecte)
                {
                    obj.ToggleVisibility();
                }
            }

            if (currentKeyboard[Key.U] && !previousKeyboard[Key.U])
            {
                foreach (MassiveObject obj in listaMassiveObj)
                {
                    obj.ToggleVisibility();
                }
            }

            // camera control (isometric mode)
            if (currentKeyboard[Key.W])
            {
                camera.MoveForward();
            }

            if (currentKeyboard[Key.S])
            {
                camera.MoveBackward();
            }

            if (currentKeyboard[Key.A])
            {
                camera.MoveLeft();
            }

            if (currentKeyboard[Key.D])
            {
                camera.MoveRight();
            }

            if (currentKeyboard[Key.Q])
            {
                camera.MoveUp();
            }

            if (currentKeyboard[Key.E])
            {
                camera.MoveDown();
            }

            if (currentKeyboard[Key.Z])
            {
                camera.Zoom();
            }

            if (currentKeyboard[Key.F])
            {
                camera.Far();
            }

            if (currentKeyboard[Key.M])
            {
                camera.FarAway();
            }

            if (currentKeyboard[Key.N])
            {
                camera.Nearly();
            }

            // object spawn
            if (currentMouse[MouseButton.Left] && !previousMouse[MouseButton.Left])
            {
                listaObiecte.Add(new Objectoid(GRAVITY, vertexuri));
            }

            // object spam cleanup
            if (currentMouse[MouseButton.Right] && !previousMouse[MouseButton.Right])
            {
                listaObiecte.Clear();
                listaMassiveObj.Clear();
            }

            // Massive Objects spam cleanup
            if (currentKeyboard[Key.X] && !previousKeyboard[Key.X])
            {
                listaMassiveObj.Clear();
            }

            // switch gravity
            if (currentKeyboard[Key.G] && !previousKeyboard[Key.G])
            {
                GRAVITY = !GRAVITY;
                /*
                foreach (Objectoid obj in listaObiecte)
                {
                    obj.DisableGravity();
                    // obj.ToggleGravity();                
                }
                */
            }

            // update falling logic 
            foreach (Objectoid obj in listaObiecte)
            {
                obj.UpdatePosition(GRAVITY);
            }

            foreach (MassiveObject obj in listaMassiveObj)
            {
                obj.UpdatePosition(GRAVITY);
            }

            // massive object spawn
            if (currentKeyboard[Key.Space] && !previousKeyboard[Key.Space])
            {
                listaMassiveObj.Add(new MassiveObject(GRAVITY));
            }

            previousKeyboard = currentKeyboard;
            previousMouse = currentMouse;
            // sfarsit cod logic 
        }

        /// <summary>
        /// are loc desenarea listei de obiecte de tip Objectoid, respectiv lista de obiecte de tip Massive Objects
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // RENDER CODE 
            grid.Draw();
            axe.DrawAxes();
            
            foreach (Objectoid obj in listaObiecte)
            {
                obj.Draw();
            }

            foreach (MassiveObject obj in listaMassiveObj)
            {
                obj.Draw();
            }

            SwapBuffers();
        }

        /// <summary>
        /// Implementare meniu
        /// </summary>
        private void DisplayHelp()
        {
            Console.WriteLine("\n   MENIU");
            Console.WriteLine(" H - meniu");
            Console.WriteLine(" ESC - parasire aplicatie");
            Console.WriteLine(" P - toggle visibility pentru axe");
            Console.WriteLine(" O - toggle visibility pentru obiecte");
            Console.WriteLine(" U - toggle visibility pentru MassiveObjects");

            Console.WriteLine(" R - resetare scena la valori implicite");
            Console.WriteLine(" B - schimbare culoare de fundal");
            Console.WriteLine(" V - schimbare vizibilitate grid");
            Console.WriteLine(" W, A, S, D, Q, E - deplasare camera izometric");
            Console.WriteLine(" Z - camera zoom");
            Console.WriteLine(" F - camera far");

            Console.WriteLine(" M - departe");
            Console.WriteLine(" N - aproape");

            Console.WriteLine(" G - manipuleaza gravitatea");
            Console.WriteLine(" Click stanga - generare obiect nou la o inaltime aleatoare");
            Console.WriteLine(" Click dreapta - curata lista de obiecte");
            Console.WriteLine(" X - curata lista de Massive Objects");

            Console.WriteLine(" Space - generare MassiveObj nou la o inaltime aleatoare");
        }

        /// <summary>
        /// laborator 5 - punctul 3 - functie ce va citi dintr-un fisier vertexurile necesare pentru obiectul de tip cub 
        /// are loc testarea de eroare la deschiderea resurselor, aruncandu-se o exceptie in cazul in care datele din fisier sunt eronate 
        /// </summary>
        /// <param name="numeFisier"></param>
        /// <returns></returns>
        public List<Vector3> citireVertexuriDinFisier(string numeFisier)
        {
            List<Vector3> vertexuriDinFisier = new List<Vector3>();

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

                        if (coordonate[i-1] < 0 || coordonate[i-1] > 250)
                        {
                            throw new ArithmeticException("Invalid vertex !");
                        }
                    }
                    vertexuriDinFisier.Add(new Vector3(coordonate[0], coordonate[1], coordonate[2]));
                }
            }

            return vertexuriDinFisier;
        }
    }
}
