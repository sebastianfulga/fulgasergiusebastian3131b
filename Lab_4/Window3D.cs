using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Lab_4
{
    public class Window3D : GameWindow
    {
        private KeyboardState previousKeyboard;
        private Randomizer random;
        
        /// <summary>
        /// laborator 4 - pentru punctele 1, 2, 3 - declarare obiect de tip Cub si axele de coordonate
        /// </summary>
        private Cub3D firstCub;
        private Camera3D camera;
        private Axes axe;

        // DEFAULTS 
        private Color DEFAULT_BACKGROUND_COLOR = Color.DeepSkyBlue;
        private bool discoModeCub = false;

        /// <summary>
        /// laborator 4 - pentru punctele 1, 2, 3 - utilizare Randomizer, afisare meniu in consola, 
        /// citire coordonate cub din fisier text folosind un constructor suprascris ce primeste ca parametru 
        /// un obiect de tip Randomizer si nume fisierului 
        /// sunt implementate si o camera si axele de coordonate
        /// </summary>
        public Window3D(): base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;
            
            random = new Randomizer();
            firstCub = new Cub3D(random, @"./../../coordonateCub3D.txt");
            camera = new Camera3D();
            axe = new Axes();

            DisplayHelp();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // set background
            GL.ClearColor(DEFAULT_BACKGROUND_COLOR);

            // set viewport
            GL.Viewport(0, 0, this.Width, this.Height);

            // set perspective 
            Matrix4 perspectiva = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Width / (float)this.Height, 1, 250);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiva);

            // set the eye - the camera 
            Matrix4 eye = Matrix4.LookAt(30, 30, 30, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref eye);
        }

        /// <summary>
        /// laborator 4 - punctele 1, 2, 3
        /// la apasarea tastei ESC se va parasi programul
        /// la apasarea tastei H se va afisa meniul
        /// la apasarea tastei R se va reseta culoarea de fundal a aplicatiei grafice 
        /// la apasarea tastei B se va genera o culoare randomizata de fundal a aplicatiei grafice
        /// la apasarea tastelor F1, F2, F3, F4, F5, F6 se va schimba culoarea fetei cubului 
        /// la apasarea tastei V se va face un toggle visibility pentru cub 
        /// la apasarea tastei C se vor afisa valorile RGB aferente cubului in consola 
        /// la apasarea tastei P se va face un toggle visibility pentru axe
        /// la apasarea tastelor W, A, S, D se va roti camera3D 
        /// la apasarea tastei X va avea loc DiscoMode pentru cub3D 
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

            if (currentKeyboard[Key.F1] && !previousKeyboard[Key.F1])
            {
                firstCub.SchimbareCuloareFata1();
            }

            if (currentKeyboard[Key.F2] && !previousKeyboard[Key.F2])
            {
                firstCub.SchimbareCuloareFata2();
            }

            if (currentKeyboard[Key.F3] && !previousKeyboard[Key.F3])
            {
                firstCub.SchimbareCuloareFata3();
            }

            if (currentKeyboard[Key.F4] && !previousKeyboard[Key.F4])
            {
                firstCub.SchimbareCuloareFata4();
            }

            if (currentKeyboard[Key.F5] && !previousKeyboard[Key.F5])
            {
                firstCub.SchimbareCuloareFata5();
            }

            if (currentKeyboard[Key.F6] && !previousKeyboard[Key.F6])
            {
                firstCub.SchimbareCuloareFata6();
            }

            if (currentKeyboard[Key.V] && !previousKeyboard[Key.V])
            {
                firstCub.ToggleVisibility();
            }

            if (currentKeyboard[Key.C] && !previousKeyboard[Key.C])
            {
                firstCub.AfisareRBGValues();
            }

            // DiscoMode 
            if (currentKeyboard[Key.X] && !previousKeyboard[Key.X])
            {
                firstCub.DiscoMode();
            }

            if (currentKeyboard[Key.W])
            {
                camera.RotateDown();
            }

            if (currentKeyboard[Key.S])
            {
                camera.RotateUp();
            }

            if (currentKeyboard[Key.A])
            {
                camera.RotateLeft();
            }

            if (currentKeyboard[Key.D])
            {
                camera.RotateRight();
            }

            if (currentKeyboard[Key.P] && !previousKeyboard[Key.P])
            {
                axe.ToggleVisibility();
            }

            if (currentKeyboard[Key.O] && !previousKeyboard[Key.O])
            {
                discoModeCub = !discoModeCub;
            }

            previousKeyboard = currentKeyboard;
            // sfarsit cod logic 
        }

        /// <summary>
        /// laborator 4 - punctul 2, modificare culori vertexuri 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // RENDER CODE 
            if (!discoModeCub)
            {
                firstCub.Draw();
            }
            
            if (discoModeCub)
            {
                firstCub.DrawVertexRGB();
            }

            axe.DrawAxes();

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
            Console.WriteLine(" B - schimbare culoare de fundal");
            Console.WriteLine(" R - resetare culoare de fundal");
            Console.WriteLine(" B - schimbare culoare de fundal - se va folosi Randomizer");
            Console.WriteLine(" V - toggle visibility pentru cub3D");
            Console.WriteLine(" P - toggle visibility pentru axe3D");
            Console.WriteLine(" C - afisare valori RGB pentru fiecare fata a cubului");
            Console.WriteLine(" F1, F2, F3, F4, F5, F6 - schimbare culoare fata cub3D - se va folosi Randomizer" +
                "\n - este necesar sa se roteasca camera pentru a putea vedea si restul fetelor cubului 3D");
            Console.WriteLine(" W, A, S, D - optiuni pentru rotire a camerei 3D");
            Console.WriteLine(" X - DiscoMode[HAHAHA]");
            Console.WriteLine(" O - toggle alt DiscoMode");
        }
    }
}
