﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Lab_9
{
    public partial class Form1 : Form
    {

        //Stări de control cameră.
        private int eyePosX, eyePosY, eyePosZ;

        private Point mousePos;
        private float camDepth;

        //Stări de control mouse.
        private bool statusControlMouse2D, statusControlMouse3D, statusMouseDown;

        //Stări de control axe de coordonate.
        private bool statusControlAxe;

        //Stări de control iluminare.
        private bool lightON;
        private bool lightON_0;

        //Stări de control obiecte 3D.
        private string statusCube;

        //Texturare.
        private int[] textures = new int[2];
        private bool brick;
        private int colorTex;

        //Stări de control obiecte 3D.
        private bool statusCubeT;
        private bool statusCubeTex1;
        private bool statusCubeTex2;
        private bool statusCubeTex3;
        private bool statusCubeTex4;

        //Structuri de stocare a vertexurilor și a listelor de vertexuri.
        private int[,] arrVertex = new int[50, 3];         //Stocam matricea de vertexuri; 3 coloane vor reține informația pentru X, Y, Z. Nr. de linii definește nr. de vertexuri.
        private int nVertex;

        private int[] arrQuadsList = new int[100];        //Lista de vertexuri pentru construirea cubului prin intermediul quadurilor. Ne bazăm pe lista de vertexuri.
        private int nQuadsList;

        private int[] arrTrianglesList = new int[100];    //Lista de vertexuri pentru construirea cubului prin intermediul triunghiurilor. Ne bazăm pe lista de vertexuri.
        private int nTrianglesList;

        /// <summary>
        /// modificare laborator 9 - punctul 3
        /// s-au modificat coordonatele din fisierul quads pentru a se crea suprafete diferite 
        /// pentru a fi mai apoi texturate conform cerintei 
        /// </summary>
        //Fișiere de in/out pentru manipularea vertexurilor.
        private string fileVertex = @"./../../vertexList.txt";
        private string fileQList = @"./../../quadsVertexList.txt";
        private string fileTList = @"./../../trianglesVertexList.txt";
        private bool statusFiles;

        //# SET 1
        private float[] valuesAmbientTemplate0 = new float[] { 0.1f, 0.1f, 0.1f, 1.0f };
        private float[] valuesDiffuseTemplate0 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        private float[] valuesSpecularTemplate0 = new float[] { 0.1f, 0.1f, 0.1f, 1.0f };
        private float[] valuesPositionTemplate0 = new float[] { 0.0f, 0.0f, 5.0f, 1.0f };
        //# SET 2
        //private float[] valuesAmbientTemplate0 = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
        //private float[] valuesDiffuseTemplate0 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        //private float[] valuesSpecularTemplate0 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        //private float[] valuesPositionTemplate0 = new float[] { 1.0f, 1.0f, 1.0f, 0.0f };

        private float[] valuesAmbient0 = new float[4];
        private float[] valuesDiffuse0 = new float[4];
        private float[] valuesSpecular0 = new float[4];
        private float[] valuesPosition0 = new float[4];

        // ON_LOAD
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SetupValues();
            SetupWindowGUI();
        }

        //   SETARI INIȚIALE
        private void SetupValues()
        {
            eyePosX = 100;
            eyePosY = 100;
            eyePosZ = 50;

            camDepth = 1.04f;

            setLight0Values();

            numericXeye.Value = eyePosX;
            numericYeye.Value = eyePosY;
            numericZeye.Value = eyePosZ;

            statusCubeTex1 = false;
            statusCubeTex2 = false;
            statusCubeTex3 = false;
            statusCubeTex4 = false;
            brick = true;
            colorTex = 0;
        }
        private void SetupWindowGUI()
        {

            setControlMouse2D(false);
            setControlMouse3D(false);

            numericCameraDepth.Value = (int)camDepth;

            setControlAxe(true);

            setCubeStatus("OFF");
            setIlluminationStatus(false);
            setSource0Status(false);

            setTrackLigh0Default();
            setColorAmbientLigh0Default();
            setColorDifuseLigh0Default();
            setColorSpecularLigh0Default();
        }

        //   MANIPULARE VERTEXURI ȘI LISTE DE COORDONATE.
        //Încărcarea coordonatelor vertexurilor și lista de compunere a obiectelor 3D.
        private void loadVertex()
        {
            try
            {
                StreamReader fileReader = new StreamReader((fileVertex));
                nVertex = Convert.ToInt32(fileReader.ReadLine().Trim());
                Console.WriteLine("Vertexuri citite: " + nVertex.ToString());

                string tmpStr = "";
                string[] str = new string[3];
                for (int i = 0; i < nVertex; i++)
                {
                    tmpStr = fileReader.ReadLine();
                    str = tmpStr.Trim().Split(' ');
                    arrVertex[i, 0] = Convert.ToInt32(str[0].Trim());
                    arrVertex[i, 1] = Convert.ToInt32(str[1].Trim());
                    arrVertex[i, 2] = Convert.ToInt32(str[2].Trim());
                }
                fileReader.Close();

            }
            catch (Exception e)
            {
                statusFiles = false;
                Console.WriteLine("Fisierul cu informații vertex <" + fileVertex + "> nu exista!");
                MessageBox.Show("Fisierul cu informații vertex <" + fileVertex + "> nu exista!");
            }
        }

        private void loadQList()
        {
            try
            {
                StreamReader fileReader = new StreamReader(fileQList);

                int tmp;
                string line;
                nQuadsList = 0;

                while ((line = fileReader.ReadLine()) != null)
                {
                    tmp = Convert.ToInt32(line.Trim());
                    arrQuadsList[nQuadsList] = tmp;
                    nQuadsList++;
                }

                fileReader.Close();
            }
            catch (Exception e)
            {
                statusFiles = false;
                MessageBox.Show("Fisierul cu informații vertex <" + fileQList + "> nu exista!");
            }

        }

        private void loadTList()
        {
            try
            {
                StreamReader fileReader = new StreamReader(fileTList);

                int tmp;
                string line;
                nTrianglesList = 0;

                while ((line = fileReader.ReadLine()) != null)
                {
                    tmp = Convert.ToInt32(line.Trim());
                    arrTrianglesList[nTrianglesList] = tmp;
                    nTrianglesList++;
                }

                fileReader.Close();
            }
            catch (Exception e)
            {
                statusFiles = false;
                MessageBox.Show("Fisierul cu informații vertex <" + fileTList + "> nu exista!");
            }

        }

        //   CONTROL CAMERĂ
        private void numericXeye_ValueChanged(object sender, EventArgs e)
        {
            eyePosX = (int)numericXeye.Value;
            GlControl1.Invalidate(); 
        }

        private void numericYeye_ValueChanged(object sender, EventArgs e)
        {
            eyePosY = (int)numericYeye.Value;
            GlControl1.Invalidate(); 
        }

        private void numericZeye_ValueChanged(object sender, EventArgs e)
        {
            eyePosZ = (int)numericZeye.Value;
            GlControl1.Invalidate(); 
        }

        private void numericCameraDepth_ValueChanged(object sender, EventArgs e)
        {
            camDepth = 1 + ((float)numericCameraDepth.Value) * 0.1f;
            GlControl1.Invalidate();
        }

        //   CONTROL MOUSE
        //Setăm variabila de stare pentru rotația în 2D a mouseului.
        private void setControlMouse2D(bool status)
        {
            if (status == false)
            {
                statusControlMouse2D = false;
                btnMouseControl2D.Text = "2D mouse control OFF";
            }
            else
            {
                statusControlMouse2D = true;
                btnMouseControl2D.Text = "2D mouse control ON";
            }
        }
        private void setControlMouse3D(bool status)
        {
            if (status == false)
            {
                statusControlMouse3D = false;
                btnMouseControl3D.Text = "3D mouse control OFF";
            }
            else
            {
                statusControlMouse3D = true;
                btnMouseControl3D.Text = "3D mouse control ON";
            }
        }
        private void btnMouseControl2D_Click(object sender, EventArgs e)
        {
            if (statusControlMouse2D == true)
            {
                setControlMouse2D(false);
            }
            else
            {
                setControlMouse3D(false);
                setControlMouse2D(true);
            }
        }
        private void btnMouseControl3D_Click(object sender, EventArgs e)
        {
            if (statusControlMouse3D == true)
            {
                setControlMouse3D(false);
            }
            else
            {
                setControlMouse2D(false);
                setControlMouse3D(true);
            }
        }

        private void GlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (statusMouseDown == true)
            {
                mousePos = new Point(e.X, e.Y);
                GlControl1.Invalidate(); 
            }
        }
        private void GlControl1_MouseDown(object sender, MouseEventArgs e)
        {
            statusMouseDown = true;
        }
        private void GlControl1_MouseUp(object sender, MouseEventArgs e)
        {
            statusMouseDown = false;
        }

        //   CONTROL ILUMINARE
        //Setăm variabila de stare pentru iluminare.
        private void setIlluminationStatus(bool status)
        {
            if (status == false)
            {
                lightON = false;
                btnLights.Text = "Iluminare OFF";
            }
            else
            {
                lightON = true;
                btnLights.Text = "Iluminare ON";
            }
        }

        private void btnLights_Click(object sender, EventArgs e)
        {
            if (lightON == false)
            {
                setIlluminationStatus(true);
            }
            else
            {
                setIlluminationStatus(false);
            }
            GlControl1.Invalidate();
        }

        private void btnLightsNo_Click(object sender, EventArgs e)
        {
            int nr = GL.GetInteger(GetPName.MaxLights);
            MessageBox.Show("Nr. maxim de lumini pentru aceasta implementare OpenGL este <" + nr.ToString() + ">.");
        }

        private void setSource0Status(bool status)
        {
            if (status == false)
            {
                lightON_0 = false;
                btnLight0.Text = "Sursa 0 OFF";
            }
            else
            {
                lightON_0 = true;
                btnLight0.Text = "Sursa 0 ON";
            }
        }

        private void btnLight0_Click(object sender, EventArgs e)
        {
            if (lightON == true)
            {
                if (lightON_0 == false)
                {
                    setSource0Status(true);
                }
                else
                {
                    setSource0Status(false);
                }
                GlControl1.Invalidate();
            }
        }

        private void setTrackLigh0Default()
        {
            trackLight0PositionX.Value = (int)valuesPosition0[0];
            trackLight0PositionY.Value = (int)valuesPosition0[1];
            trackLight0PositionZ.Value = (int)valuesPosition0[2];
        }
        private void trackLight0PositionX_Scroll(object sender, EventArgs e)
        {
            valuesPosition0[0] = trackLight0PositionX.Value;
            GlControl1.Invalidate();
        }
        private void trackLight0PositionY_Scroll(object sender, EventArgs e)
        {
            valuesPosition0[1] = trackLight0PositionY.Value;
            GlControl1.Invalidate();
        }
        private void trackLight0PositionZ_Scroll(object sender, EventArgs e)
        {
            valuesPosition0[2] = trackLight0PositionZ.Value;
            GlControl1.Invalidate();
        }
        private void setColorAmbientLigh0Default()
        {
            numericLight0Ambient_Red.Value = (decimal)valuesAmbient0[0];
            numericLight0Ambient_Green.Value = (decimal)valuesAmbient0[1];
            numericLight0Ambient_Blue.Value = (decimal)valuesAmbient0[2];
        }
        private void numericLight0Ambient_Red_ValueChanged(object sender, EventArgs e)
        {
            valuesAmbient0[0] = (float)numericLight0Ambient_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Ambient_Green_ValueChanged(object sender, EventArgs e)
        {
            valuesAmbient0[1] = (float)numericLight0Ambient_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Ambient_Blue_ValueChanged(object sender, EventArgs e)
        {
            valuesAmbient0[2] = (float)numericLight0Ambient_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        private void setColorDifuseLigh0Default()
        {
            numericLight0Difuse_Red.Value = (decimal)valuesDiffuse0[0];
            numericLight0Difuse_Green.Value = (decimal)valuesDiffuse0[1];
            numericLight0Difuse_Blue.Value = (decimal)valuesDiffuse0[2];
        }
        private void numericLight0Difuse_Red_ValueChanged(object sender, EventArgs e)
        {
            valuesDiffuse0[0] = (float)numericLight0Difuse_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Difuse_Green_ValueChanged(object sender, EventArgs e)
        {
            valuesDiffuse0[1] = (float)numericLight0Difuse_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Difuse_Blue_ValueChanged(object sender, EventArgs e)
        {
            valuesDiffuse0[2] = (float)numericLight0Difuse_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        private void setColorSpecularLigh0Default()
        {
            numericLight0Specular_Red.Value = (decimal)valuesSpecular0[0];
            numericLight0Specular_Green.Value = (decimal)valuesSpecular0[1];
            numericLight0Specular_Blue.Value = (decimal)valuesSpecular0[2];
        }
        private void numericLight0Specular_Red_ValueChanged(object sender, EventArgs e)
        {
            valuesSpecular0[0] = (float)numericLight0Specular_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Specular_Green_ValueChanged(object sender, EventArgs e)
        {
            valuesSpecular0[1] = (float)numericLight0Specular_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Specular_Blue_ValueChanged(object sender, EventArgs e)
        {
            valuesSpecular0[2] = (float)numericLight0Specular_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        private void setLight0Values()
        {
            for (int i = 0; i < valuesAmbientTemplate0.Length; i++)
            {
                valuesAmbient0[i] = valuesAmbientTemplate0[i];
            }
            for (int i = 0; i < valuesDiffuseTemplate0.Length; i++)
            {
                valuesDiffuse0[i] = valuesDiffuseTemplate0[i];
            }
            for (int i = 0; i < valuesPositionTemplate0.Length; i++)
            {
                valuesPosition0[i] = valuesPositionTemplate0[i];
            }
        }
        private void btnLight0Reset_Click(object sender, EventArgs e)
        {
            setLight0Values();
            setTrackLigh0Default();
            setColorAmbientLigh0Default();
            setColorDifuseLigh0Default();
            setColorSpecularLigh0Default();
            GlControl1.Invalidate();
        }

        //   CONTROL OBIECTE 3D
        private void setControlAxe(bool status)
        {
            if (status == false)
            {
                statusControlAxe = false;
                btnShowAxes.Text = "Axe Oxyz OFF";
            }
            else
            {
                statusControlAxe = true;
                btnShowAxes.Text = "Axe Oxyz ON";
            }
        }
        private void btnShowAxes_Click(object sender, EventArgs e)
        {
            if (statusControlAxe == true)
            {
                setControlAxe(false);
            }
            else
            {
                setControlAxe(true);
            }
            GlControl1.Invalidate();
        }

        private void setCubeStatus(string status)
        {
            if (status.Trim().ToUpper().Equals("TRIANGLES"))
            {
                statusCube = "TRIANGLES";
            }
            else if (status.Trim().ToUpper().Equals("QUADS"))
            {
                statusCube = "QUADS";
            }
            else
            {
                statusCube = "OFF";
            }
        }
        private void btnCubeQ_Click(object sender, EventArgs e)
        {
            statusFiles = true;
            loadVertex();
            loadQList();
            setCubeStatus("QUADS");
            GlControl1.Invalidate();
        }
        private void btnCubeT_Click(object sender, EventArgs e)
        {
            statusFiles = true;
            loadVertex();
            loadTList();
            setCubeStatus("TRIANGLES");
            GlControl1.Invalidate();
        }
        private void btnResetObjects_Click(object sender, EventArgs e)
        {
            setCubeStatus("OFF");
            GlControl1.Invalidate();
        }

        //   ADMINISTRARE MOD 3D (METODA PRINCIPALĂ)
        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            GL.ClearColor(Color.Black);

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)camDepth, 4 / 3, 1, 10000); 
            Matrix4 lookat = Matrix4.LookAt(eyePosX, eyePosY, eyePosZ, 0, 0, 0, 0, 1, 0); 
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview); 
            GL.LoadIdentity();
            GL.LoadMatrix(ref lookat);
            GL.Viewport(0, 0, GlControl1.Width, GlControl1.Height); 
            GL.Enable(EnableCap.DepthTest); 
            GL.DepthFunc(DepthFunction.Less); 

            if (lightON == true)
            {
                GL.Enable(EnableCap.Lighting);
            }
            else
            {
                GL.Disable(EnableCap.Lighting);
            }

            GL.Light(LightName.Light0, LightParameter.Ambient, valuesAmbient0);
            GL.Light(LightName.Light0, LightParameter.Diffuse, valuesDiffuse0);
            GL.Light(LightName.Light0, LightParameter.Specular, valuesSpecular0);
            GL.Light(LightName.Light0, LightParameter.Position, valuesPosition0);

            if ((lightON == true) && (lightON_0 == true))
            {
                GL.Enable(EnableCap.Light0);
            }
            else
            {
                GL.Disable(EnableCap.Light0);
            }

            if (statusControlMouse2D == true)
            {
                GL.Rotate(mousePos.X, 0, 1, 0);
            }

            if (statusControlMouse3D == true)
            {
                GL.Rotate(mousePos.X, 0, 1, 1);
            }

            // Texturare 
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            LoadTextures();

            if (statusControlAxe == true)
            {
                DeseneazaAxe();
            }

            if (statusCube.ToUpper().Equals("QUADS"))
            {
                DeseneazaCubQ();
            }
            else if (statusCube.ToUpper().Equals("TRIANGLES"))
            {
                DeseneazaCubT();
            }

            if (statusCubeTex1 == true)
            {
                DeseneazaCubQ_Tex1();
            }
            if (statusCubeTex2 == true)
            {
                DeseneazaCubQ_Tex2();
            }
            if (statusCubeTex3 == true)
            {
                DeseneazaCubT_Tex3();
            }
            if (statusCubeTex4 == true)
            {
                DeseneazaCubT_Tex4();
            }

            GlControl1.SwapBuffers();
        }

        // DESENARE OBIECTE 3D
        //Desenează axe XYZ.
        private void DeseneazaAxe()
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(75, 0, 0);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 75, 0);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 75);
            GL.End();
        }

        /// <summary>
        /// modificare laborator 9 - punctul 3 
        /// </summary>
        private void DeseneazaCubQ()
        {
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < nQuadsList; i++)
            {
                switch (i % 4)
                {
                    case 0:
                        GL.Color3(Color.Blue);
                        break;
                    case 1:
                        GL.Color3(Color.Red);
                        break;
                    case 2:
                        GL.Color3(Color.Green);
                        break;
                    case 3:
                        GL.Color3(Color.Yellow);
                        break;
                }
                int x = arrQuadsList[i];
                GL.Vertex3(arrVertex[x, 0], arrVertex[x, 1], arrVertex[x, 2]);
            }
            GL.End();
        }

        /// <summary>
        /// modificare laborator 9 - punctul 3 
        /// </summary>
        private void DeseneazaCubT()
        {
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < nTrianglesList; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        GL.Color3(Color.Blue);
                        break;
                    case 1:
                        GL.Color3(Color.Red);
                        break;
                    case 2:
                        GL.Color3(Color.Green);
                        break;
                }
                int x = arrTrianglesList[i];
                GL.Vertex3(arrVertex[x, 0], arrVertex[x, 1], arrVertex[x, 2]);
            }
            GL.End();
        }

        // Texturare 
        private void LoadTextures()
        {
            GL.GenTextures(textures.Length, textures);
            LoadTexture(textures[0], @"./../../brickTexture.jpg");
            LoadTexture(textures[1], @"./../../OpenGLTexture.png");
        }

        private void LoadTexture(int textureId, string filename)
        {
            Bitmap bmp = new Bitmap(filename);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                          bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                          PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
        }

        private void rbTexture0_CheckedChanged(object sender, EventArgs e)
        {
            brick = true;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            brick = false;
        }

        private void rbColorWhite_CheckedChanged(object sender, EventArgs e)
        {
            colorTex = 0;
        }
        private void rbColorRed_CheckedChanged(object sender, EventArgs e)
        {
            colorTex = 1;
        }
        private void rbColorBlue_CheckedChanged(object sender, EventArgs e)
        {
            colorTex = 2;
        }
        private void btnTexT_1_Click(object sender, EventArgs e)
        {
            resetTexCubes();
            loadVertex();
            loadTList();
            statusCubeTex3 = true;
            GlControl1.Invalidate();
        }

        private void btnTexT_2_Click(object sender, EventArgs e)
        {
            resetTexCubes();
            loadVertex();
            loadTList();
            statusCubeTex4 = true;
            GlControl1.Invalidate();
        }

        private void btnReset_Tex2_Click(object sender, EventArgs e)
        {
            resetTexCubes();
        }
        private void btnTexQ_1_Click(object sender, EventArgs e)
        {
            resetTexCubes();
            loadVertex();
            loadQList();
            statusCubeTex1 = true;
            GlControl1.Invalidate();
        }

        private void btnTexQ_2_Click(object sender, EventArgs e)
        {
            resetTexCubes();
            loadVertex();
            loadQList();
            statusCubeTex2 = true;
            GlControl1.Invalidate();
        }

        private void btnReset_Tex1_Click(object sender, EventArgs e)
        {
            resetTexCubes();
        }

        private void resetTexCubes()
        {
            setCubeStatus("OFF");
            statusCubeTex1 = false;
            statusCubeTex2 = false;
            statusCubeTex3 = false;
            statusCubeTex4 = false;
            GlControl1.Invalidate();
        }

        /// <summary>
        ///  modificare laborator 9 - punctul 3
        /// </summary>
        private void DeseneazaCubQ_Tex1()
        {
            if (brick == true)
            {
                GL.BindTexture(TextureTarget.Texture2D, textures[0]);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, textures[1]);
            }

            switch (colorTex)
            {
                case 0:
                    GL.Color3(Color.White); 
                    break;
                case 1:
                    GL.Color3(Color.FromArgb(0, 255, 0, 0));
                    break;
                case 2:
                    GL.Color3(Color.FromArgb(0, 0, 0, 255));
                    break;
            }

            //GL.Normal3(0.0, 0.0, 1.0);
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < nQuadsList; i = i + 4)
            {
                GL.TexCoord2(0.0, 1.0);
                GL.Vertex3(arrVertex[arrQuadsList[i], 0], arrVertex[arrQuadsList[i], 1], arrVertex[arrQuadsList[i], 2]);
                GL.TexCoord2(1.0, 1.0);
                GL.Vertex3(arrVertex[arrQuadsList[i + 1], 0], arrVertex[arrQuadsList[i + 1], 1], arrVertex[arrQuadsList[i + 1], 2]);
                GL.TexCoord2(1.0, 0.0);
                GL.Vertex3(arrVertex[arrQuadsList[i + 2], 0], arrVertex[arrQuadsList[i + 2], 1], arrVertex[arrQuadsList[i + 2], 2]);
                GL.TexCoord2(0.0, 0.0);
                GL.Vertex3(arrVertex[arrQuadsList[i + 3], 0], arrVertex[arrQuadsList[i + 3], 1], arrVertex[arrQuadsList[i + 3], 2]);
            }
            GL.End();
        }

        /// <summary>
        /// modificare laborator 9 - punctul 3 
        /// </summary>
        private void DeseneazaCubQ_Tex2()
        {
            if (brick == true)
            {
                GL.BindTexture(TextureTarget.Texture2D, textures[0]);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, textures[1]);
            }

            switch (colorTex)
            {
                case 0:
                    GL.Color3(Color.White); 
                    break;
                case 1:
                    GL.Color3(Color.FromArgb(0, 255, 0, 0));
                    break;
                case 2:
                    GL.Color3(Color.FromArgb(0, 0, 0, 255));
                    break;
            }

            //GL.Normal3(0.0, 0.0, 1.0);
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < nQuadsList; i = i + 4)
            {
                GL.TexCoord2(0.0, 0.5);
                GL.Vertex3(arrVertex[arrQuadsList[i], 0], arrVertex[arrQuadsList[i], 1], arrVertex[arrQuadsList[i], 2]);
                GL.TexCoord2(0.5, 0.5);
                GL.Vertex3(arrVertex[arrQuadsList[i + 1], 0], arrVertex[arrQuadsList[i + 1], 1], arrVertex[arrQuadsList[i + 1], 2]);
                GL.TexCoord2(0.5, 0.0);
                GL.Vertex3(arrVertex[arrQuadsList[i + 2], 0], arrVertex[arrQuadsList[i + 2], 1], arrVertex[arrQuadsList[i + 2], 2]);
                GL.TexCoord2(0.0, 0.0);
                GL.Vertex3(arrVertex[arrQuadsList[i + 3], 0], arrVertex[arrQuadsList[i + 3], 1], arrVertex[arrQuadsList[i + 3], 2]);
            }
            GL.End();
        }

        /// <summary>
        /// modificare laborator 9 - punctul 3 
        /// </summary>
        private void DeseneazaCubT_Tex3()
        {
            if (brick == true)
            {
                GL.BindTexture(TextureTarget.Texture2D, textures[0]);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, textures[1]);
            }

            switch (colorTex)
            {
                case 0:
                    GL.Color3(Color.White); 
                    break;
                case 1:
                    GL.Color3(Color.FromArgb(0, 255, 0, 0));
                    break;
                case 2:
                    GL.Color3(Color.FromArgb(0, 0, 0, 255));
                    break;
            }

            //GL.Normal3(0.0, 0.0, 1.0);
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < nTrianglesList; i = i + 6)
            {
                GL.TexCoord2(0.0, 0.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i], 0], arrVertex[arrTrianglesList[i], 1], arrVertex[arrTrianglesList[i], 2]);
                GL.TexCoord2(1.0, 0.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 1], 0], arrVertex[arrTrianglesList[i + 1], 1], arrVertex[arrTrianglesList[i + 1], 2]);
                GL.TexCoord2(0.0, 1.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 2], 0], arrVertex[arrTrianglesList[i + 2], 1], arrVertex[arrTrianglesList[i + 2], 2]);
                GL.TexCoord2(1.0, 0.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 3], 0], arrVertex[arrTrianglesList[i + 3], 1], arrVertex[arrTrianglesList[i + 3], 2]);
                GL.TexCoord2(0.0, 1.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 4], 0], arrVertex[arrTrianglesList[i + 4], 1], arrVertex[arrTrianglesList[i + 4], 2]);
                GL.TexCoord2(1.0, 1.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 5], 0], arrVertex[arrTrianglesList[i + 5], 1], arrVertex[arrTrianglesList[i + 5], 2]);
            }
            GL.End();
        }

        /// <summary>
        /// modificare laborator 9 - punctul 3 
        /// </summary>
        private void DeseneazaCubT_Tex4()
        {
            if (brick == true)
            {
                GL.BindTexture(TextureTarget.Texture2D, textures[0]);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, textures[1]);
            }

            switch (colorTex)
            {
                case 0:
                    GL.Color3(Color.White); 
                    break;
                case 1:
                    GL.Color3(Color.FromArgb(0, 255, 0, 0));
                    break;
                case 2:
                    GL.Color3(Color.FromArgb(0, 0, 0, 255));
                    break;
            }

            //GL.Normal3(0.0, 0.0, 1.0);
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < nTrianglesList; i = i + 6)
            {
                GL.TexCoord2(0.0, 0.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i], 0], arrVertex[arrTrianglesList[i], 1], arrVertex[arrTrianglesList[i], 2]);
                GL.TexCoord2(0.5, 0.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 1], 0], arrVertex[arrTrianglesList[i + 1], 1], arrVertex[arrTrianglesList[i + 1], 2]);
                GL.TexCoord2(0.0, 0.5);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 2], 0], arrVertex[arrTrianglesList[i + 2], 1], arrVertex[arrTrianglesList[i + 2], 2]);
                GL.TexCoord2(0.5, 0.0);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 3], 0], arrVertex[arrTrianglesList[i + 3], 1], arrVertex[arrTrianglesList[i + 3], 2]);
                GL.TexCoord2(0.0, 0.5);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 4], 0], arrVertex[arrTrianglesList[i + 4], 1], arrVertex[arrTrianglesList[i + 4], 2]);
                GL.TexCoord2(0.5, 0.5);
                GL.Vertex3(arrVertex[arrTrianglesList[i + 5], 0], arrVertex[arrTrianglesList[i + 5], 1], arrVertex[arrTrianglesList[i + 5], 2]);
            }
            GL.End();
        }
    }
}
