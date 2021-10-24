using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.IO;

namespace Immediate_Mode_OpenTK
{
    /// <summary>
    /// tema laborator 3 
    /// punctele 1, 5, 8 si 9 
    /// </summary>
    class ImmediateMode : GameWindow
    {
        private const int XYZ_SIZE = 75;

        /// <summary>
        /// Modificare tema laborator 3 - declararea culorilor implicite pentru triunghi 
        /// </summary>
        private Color color1 = Color.MidnightBlue;
        private Color color2 = Color.Honeydew;
        private Color color3 = Color.IndianRed;

        /// <summary>
        /// variabila de tip bool transperanta are rolul de a schimba transparenta triunghiului
        /// </summary>
        private bool transperanta = false;

        /// <summary>
        /// path-ul implicit este \fulgasergiusebastian3131b\Immediate_Mode\bin\Debug\coordonate.txt 
        /// </summary>
        private readonly string numeFisier = "coordonate.txt";
        private readonly float[] coordonate = new float[9];

        /// <summary>
        /// Modificare pentru laborator 3 - punctul 8 
        /// rotire Camera cu ajutorul mouse-ului
        /// </summary>

        MouseState originalMouseState;

        float speed = 1.5f;

        Vector3 position = new Vector3(0.0f, 0.0f, 3.0f);
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

        private bool firstMove = true;
        private bool firstMouse = true;
        private Vector2 lastPos;

        public ImmediateMode() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            Console.WriteLine("OpenGl versiunea: " + GL.GetString(StringName.Version));
            Title = "OpenGl versiunea: " + GL.GetString(StringName.Version) + " (mod imediat)";

            Console.WriteLine("\nOptiuni pentru schimbare culori: ");

            Console.WriteLine("LControl - resetare culori initiale");
            Console.WriteLine("R - gradient(Red, Honeydew, IndianRed");
            Console.WriteLine("B - gradient(DarkBlue, MidnightBlue, CornflowerBlue)");
            Console.WriteLine("Space - gradient(Firebrick, Bisque, Purple)");

            Console.WriteLine("Apasati tasta ESCAPE pentru a iesi ...");
        }

        /// <summary>
        /// setare mediu OpenGL si incarcarea resurselor 
        /// </summary>
        /// <param name="e"></param>        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.AliceBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

            // citire coordonate din fisier text 
            using (StreamReader sr = new StreamReader(numeFisier))
            {
                int i = 0;
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    var numere = linie.Split();
                    foreach (var nr in numere)
                    {
                        coordonate[i++] = float.Parse(nr);
                    }
                }
            }

            lastPos = new Vector2(Mouse.X, Mouse.Y);
        }

        /// <summary>
        /// initierea afisarii si setarea viewport-ului grafic 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            Matrix4 lookat = Matrix4.LookAt(30, 30, 30, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            // pozitia camerei 
            Vector3 cameraPos = new Vector3(0.0f, 0.0f, 3.0f);
            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(cameraPos - cameraTarget);

            Vector3 up = Vector3.UnitY;
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));

            Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);

            Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f),
             new Vector3(0.0f, 0.0f, 0.0f),
             new Vector3(0.0f, 1.0f, 0.0f));

            view = Matrix4.LookAt(position, position + front, cameraUp);
            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadMatrix(ref view);
        }

        /// <summary>
        /// sectiunea pentru game logic / business logic 
        /// </summary>
        /// <param name="e"></param>        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyboard[Key.Escape])
            {
                Exit();
                return;
            }

            // modificare pentru tema laborator 3 - punctele 5 / 8 / 9 
            // la apasarea unui set de taste se va modifica culoarea unui triunghi
            // coordonatele sunt incarcate dintr-un fisier text la inceputul programului 
            if (keyboard[Key.LControl])
            {
                color1 = Color.MidnightBlue;
                color2 = Color.Honeydew;
                color3 = Color.IndianRed;
            }

            if (keyboard[Key.R])
            {
                color1 = Color.Red;
                color2 = Color.Honeydew;
                color3 = Color.IndianRed;
            }

            if (keyboard[Key.B])
            {
                color1 = Color.DarkBlue;
                color2 = Color.MidnightBlue;
                color3 = Color.CornflowerBlue;
            }

            if (keyboard[Key.Space])
            {
                color1 = Color.Firebrick;
                color2 = Color.Bisque;
                color3 = Color.Purple;
            }

            if (keyboard[Key.F1])
            {
                transperanta = true;
            }

            if (keyboard[Key.F2])
            {
                transperanta = false;
            }

            if (keyboard[Key.W])
            {
                position += front * speed * (float)e.Time;
            }

            if (keyboard[Key.S])
            {
                position -= front * speed * (float)e.Time;
            }

            if (keyboard[Key.A])
            {
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time;
            }

            if (keyboard[Key.D])
            {
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time;
            }

            if (keyboard[Key.P])
            {
                position += up * speed * (float)e.Time;
            }

            if (keyboard[Key.LShift])
            {
                position -= up * speed * (float)e.Time;
            }

            /// implementare laborator 3 - punct 8 
            /// se va modifica unghiul camerei cu ajutorul mouse-ului 

            if (firstMouse)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }

            if (firstMove)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                float deltaX = mouse.X - lastPos.X;
                float deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);
            }
        }

        /** Secțiunea pentru randarea scenei 3D. Controlată de modulul logic din metoda ONUPDATEFRAME().
            Parametrul de intrare "e" conține informatii de timing pentru randare. */
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            DrawAxes();
            DrawObjects();

            // Se lucrează în modul DOUBLE BUFFERED - câtă vreme se afișează o imagine randată, o alta se randează în background apoi cele 2 sunt schimbate...
            SwapBuffers();
        }

        /// Laborator 3 - punctul 1
        /// Desenați axele de coordonate din aplicația template folosind un singur apel GL.Begin(). 
        private void DrawAxes()
        {
            
            //GL.LineWidth(3.0f);

            // Desenează axa Ox (cu roșu).
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(XYZ_SIZE, 0, 0);

            // Desenează axa Oy (cu galben).
            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, XYZ_SIZE, 0); ;

            // Desenează axa Oz (cu verde).
            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, XYZ_SIZE);

            GL.End();
        }

        /// <summary>
        ///  desenare triunghi 
        /// </summary>
        private void DrawObjects()
        {
            if (!transperanta)
            {
                GL.Begin(PrimitiveType.Triangles);

                GL.Color3(color1);
                // GL.Vertex3(15.0f, 0.0f, 0.0f);
                GL.Vertex3(coordonate[0], coordonate[1], coordonate[2]);

                GL.Color3(color2);
                // GL.Vertex3(0.0f, 15.0f, 0.0f);
                GL.Vertex3(coordonate[3], coordonate[4], coordonate[5]);

                GL.Color3(color3);
                // GL.Vertex3(0.0f, 0.0f, 15.0f);
                GL.Vertex3(coordonate[6], coordonate[7], coordonate[8]);

                GL.End();
            }
            /// utilizarea canalului de transparenta - punctul 8 
            else
            {
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
                GL.Enable(EnableCap.Blend);

                GL.Begin(PrimitiveType.Triangles);

                GL.Color4(color1.R, color1.G, color1.B, Convert.ToByte(127));
                // GL.Vertex3(15.0f, 0.0f, 0.0f);
                GL.Vertex3(coordonate[0], coordonate[1], coordonate[2]);

                GL.Color4(color2.R, color2.G, color2.B, Convert.ToByte(127));
                // GL.Vertex3(0.0f, 15.0f, 0.0f);
                GL.Vertex3(coordonate[3], coordonate[4], coordonate[5]);

                GL.Color4(color3.R, color3.G, color3.B, Convert.ToByte(127));
                // GL.Vertex3(0.0f, 0.0f, 15.0f);
                GL.Vertex3(coordonate[6], coordonate[7], coordonate[8]);

                GL.End();
                GL.Disable(EnableCap.Blend);
            }

            logare();
        }

        /// <summary>
        /// metoda pentru logare in consola si fisier text + rezolvare laborator 3 - punctul 9 
        /// fisierul va fi sters la resetarea programului 
        /// de mentionat ca fisierul se afla in path-ul \fulgasergiusebastian3131b\Immediate_Mode\bin
        /// </summary>
        private void logare()
        {
            Console.WriteLine(color1);
            Console.WriteLine("Culori RBG pentru primul vertex: " + color1.R + " " + color1.G + " " + color1.B);

            Console.WriteLine(color2);
            Console.WriteLine("Culori RBG pentru al doilea vertex: " + color2.R + " " + color2.G + " " + color2.B);

            Console.WriteLine(color3);
            Console.WriteLine("Culori RBG pentru al treilea vertex: " + color3.R + " " + color3.G + " " + color3.B);

            using (StreamWriter fisierLog = new StreamWriter("log.txt", true))
            {
                fisierLog.WriteLine(color1);
                fisierLog.WriteLine("Culori RBG pentru primul vertex: " + color1.R + " " + color1.G + " " + color1.B);

                fisierLog.WriteLine(color2);
                fisierLog.WriteLine("Culori RBG pentru al doilea vertex: " + color2.R + " " + color2.G + " " + color2.B);

                fisierLog.WriteLine(color3);
                fisierLog.WriteLine("Culori RBG pentru al treilea vertex: " + color3.R + " " + color3.G + " " + color3.B);
            }
        }

        [STAThread]
        static void Main(string[] args)
        {

            /**Utilizarea cuvântului-cheie "using" va permite dealocarea memoriei o dată ce obiectul nu mai este
               în uz (vezi metoda "Dispose()").
               Metoda "Run()" specifică cerința noastră de a avea 30 de evenimente de tip UpdateFrame per secundă
               și un număr nelimitat de evenimente de tip randare 3D per secundă (maximul suportat de subsistemul
               grafic). Asta nu înseamnă că vor primi garantat respectivele valori!!!
               Ideal ar fi ca după fiecare UpdateFrame să avem si un RenderFrame astfel încât toate obiectele generate
               în scena 3D să fie actualizate fără pierderi (desincronizări între logica aplicației și imaginea randată
               în final pe ecran). */

            File.WriteAllText(@"log.txt", string.Empty);

            using (ImmediateMode example = new ImmediateMode())
            {
                example.Run(30.0, 0.0);
            }
        }
    }
}
