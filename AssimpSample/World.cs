// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Threading;

namespace AssimpSample
{

    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        /// <summary>
        ///	 Ugao rotacije Meseca
        /// </summary>
        private float m_moonRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije Zemlje
        /// </summary>
        private float m_earthRotation = 0.0f;

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 6000.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;
        private OpenGL gl;

        public double scaleFactor = 1.0;
        public double translateFactor = 0.0;

        public bool isAnimation = false;

        private enum TextureObjects { Wood = 0, Carpet };
        private readonly int m_textureCount = Enum.GetNames(typeof(TextureObjects)).Length;

        public uint[] m_textures = null;

        public string[] m_textureFiles = { "..//..//3D Models//Textures//wood.jpg", "..//..//3D Models//Textures//ocarpet.jpg" };

        public float color_r = 0.4f;
        public float color_g = 0.4f;
        public float color_b = 0.4f;
        public float animationSpeed = 1.0f;
        public DispatcherTimer timer = new DispatcherTimer();

        public bool ejectHolder = true;
        public bool rotateDisc = true;
        public bool ejectDisc = false;
        public bool slowerRotate = false;
        public bool stopRotate = false;

        private float animationFX11 = 0.0f;
        private float animationFX12 = 0.0f;
        private float animationF1Z = 0.0f;

        private float animationFX21 = 0.0f;
        private float animationFX22 = 0.0f;
        private float animationF2R = 10000f;
        private float animationF3Y = 0;
        private float animationF3X = 0;
        private float animationF3R = 0.0f;
        private float animationF4Y = 0.0f;
        private float animationF5Z = 0.0f;

        public float  discRotationSpeed = 0.0f;
        private float translateOffset = -300.0f;

        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>


        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String sceneFileName, int width, int height, OpenGL gl)
        {
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_width = width;
            this.m_height = height;
            m_textures = new uint[m_textureCount];

            this.gl = gl;
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode
        private void Animate()
        {
            timer.Interval = TimeSpan.FromMilliseconds(10 * (1.0 / animationSpeed));
            timer.Tick += new EventHandler(startAnimations);
        }

        private void startAnimations(object sender, EventArgs e)
        {
            if (isAnimation)
            {
                if (ejectHolder && rotateDisc)
                {
                    if (animationF1Z < 180.0f)  
                    {
                        animationF1Z += 5.0f;
                    }
                    else    
                    {
                        //slowerRotate = true;
                        //// TODO ukloniti
                        //isAnimation = false;
                        //animationF1Z = 0.0f;

                        if (animationF2R > 0)
                        {
                            animationF2R -= 50.0f;
                        }
                        else
                        {
                            if (animationF3Y < 70.0f)
                            {
                                animationF3Y += 2.0f;
                                animationF3X += 5.0f;
                            }
                            else
                            {
                                if (animationF4Y < 361.0f)
                                {
                                    animationF4Y += 5.0f;
                                }
                                else
                                {
                                    if (animationF5Z < 185.0f)
                                    {
                                        animationF5Z += 5.0f;
                                    }
                                    else
                                    {
                                        isAnimation = false;
                                        animationF1Z = 0.0f;
                                        animationF2R = 10000f;
                                        animationF3X = 0.0f;
                                        animationF3Y = 0.0f;
                                        animationF3R = 0.0f;
                                        animationF4Y = 0.0f;
                                        animationF5Z = 0.0f;
                                    }
                                }

                            }

                        }
                    }
                }
            }
        }
        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            Animate();

            float[] white_light = { 1.0f, 1.0f, 1.0f, 1.0f };

            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            gl.LightModel(LightModelParameter.Ambient, white_light);

            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 0f, 0f);

            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_FLAT);
            //ukljuceno testiranje dubine i skrivanje nevidljviih povrsina
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);

            // ukljucen color tracking
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);

            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);

            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_ADD);

            SetupTexture(gl);

            m_scene.LoadScene();
            m_scene.Initialize();
        }





        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Redraw()
        {
            Draw(this.gl);
        }
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //gl.FrontFace(OpenGL.GL_CCW);
            gl.Viewport(0, 0, m_width, m_height);

            gl.PushMatrix();

            gl.Translate(0.0f, 0.0f, -m_sceneDistance);
            //zbudzeno da prikazuje i BOCNU i ZADNJU STRANU 
            gl.LookAt(-60.0f, 40f, -120f, 0, -1, 0, 0, 1, 0);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            gl.PushMatrix();
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Carpet]);
            gl.Normal(0.0f, 1.0f, 0.0f);
            gl.Color(0.71, 0.73, 0.75);
            gl.Translate(0.0f, 0.0f, 0.0f);
            gl.Scale(5000.0f, 5000.0f, 5000.0f);
            drawFloor(gl);
            gl.PopMatrix();

            // vertex u centru podloge 
            // vertex u centru podloge + 10*normala

            gl.PushMatrix();
            Cube table = new Cube();

            gl.PushMatrix();
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Wood]);
            gl.Translate(0.0f, 402.0f, -20.0f);
            gl.Scale(800.0f, 400.0f, 440.0f);
            gl.Color(0.30f, 0.15f, 0.00f);
            table.Render(gl, RenderMode.Render);
            gl.PopMatrix();

            gl.Disable(OpenGL.GL_TEXTURE_2D);
            //iscrtavanje kompa

            gl.PushMatrix();


            //gl.PushMatrix();
            //gl.Translate(translateFactor, 0.0f, 0.0f);
            //gl.Translate(-300.0f, 820.0f, 0.0f);
            //gl.Scale(100.0f, 100.0f, 100.0f);  
            //gl.Scale(scaleFactor, scaleFactor, scaleFactor);

            //m_scene.Draw();
            //gl.PopMatrix();




            //Cube disk_holder = new Cube();

            //gl.PushMatrix(); 
            //gl.Translate(translateFactor, 0.0f, 0.0f);
            //gl.Translate(-630.0f, 1100.0f, 90.0f + animationF1Z);
            //gl.Scale(scaleFactor, scaleFactor, scaleFactor);
            //gl.Scale(70.0f, 8.0f, 180.0f);
            //gl.Color(0.329412, 0.329412, 0.329412);
            //disk_holder.Render(gl, RenderMode.Render);
            //gl.PopMatrix();


            //gl.PushMatrix();
            //Disk disk = new Disk();
            //disk.InnerRadius = 2.0f;
            //disk.OuterRadius = 10.0f;
            //disk.CreateInContext(gl);
            //gl.Translate(translateFactor, 0.0f, 0.0f);
            //gl.Translate(-630.0f, 1112.0f, 180.0f + animationF1Z);
            //gl.Scale(7.3f, 7.3f, 7.3f);
            //gl.Rotate(90.0f, 180.0f, 0.0f);
            //gl.Color(255.0f, 255.0f, 255.0f);
            //disk.Render(gl, RenderMode.Render);
            //gl.PopMatrix();

            gl.Translate(translateFactor + translateOffset, 820.0, -20.0f);
            gl.Scale(scaleFactor, scaleFactor, scaleFactor);
            drawComputer(gl);
            gl.PopMatrix();

            SetupLightRed(gl);


            

            gl.PopMatrix();

            gl.PopMatrix();

            gl.PushMatrix();
            gl.Viewport(m_width - 147, 0, m_width, m_height);
            gl.DrawText(0, 72, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Predmet: Racunarska grafika");
            gl.DrawText(0, 72, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "_______________________");
            gl.DrawText(0, 59, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Sk.god: 2020/21.");
            gl.DrawText(0, 59, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "______________");
            gl.DrawText(0, 46, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Ime: Aleksandar");
            gl.DrawText(0, 46, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "_____________");
            gl.DrawText(0, 33, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Prezime: Ignjatijevic");
            gl.DrawText(0, 33, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "________________");
            gl.DrawText(0, 20, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Sifra zad: 6.2");
            gl.DrawText(0, 20, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "___________");
            gl.PopMatrix();

            SetupLight(gl);

            gl.Enable(OpenGL.GL_TEXTURE_2D);

            gl.Flush();
        }

        private void drawFloor(OpenGL gl)
        {
            float[] vertices = {
                -0.5f, 0.0f, -0.5f, 0.0f, 1.0f, // LU
                -0.5f, 0.0f, 0.5f, 0.0f, 0.0f,// LD
                0.5f, 0.0f, 0.5f, 1.0f, 0.0f,// RD
                0.5f, 0.0f, -0.5f, 1.0f, 1.0f// RU
            };
            gl.Begin(OpenGL.GL_QUADS);
            for(int i = 0; i < vertices.Length; i += 5)
            {
                gl.TexCoord(vertices[i+3], vertices[i+4]);
                gl.Vertex(vertices[i], vertices[i + 1], vertices[i + 2]);
            }
            gl.End();
        }

        private void drawComputerModel(OpenGL gl) 
        {
            gl.PushMatrix();
            //gl.Translate(-300.0f, 820.0f, 0.0f);
            gl.Scale(100.0f, 100.0f, 100.0f);
            m_scene.Draw();
            gl.PopMatrix();
        }
        private void drawDiskAndHolder(OpenGL gl)
        {
            Cube disk_holder = new Cube();

            gl.PushMatrix();
            gl.Translate(-330.0f, 280.0f, 90.0f + animationF1Z - animationF5Z);
            gl.Scale(70.0f, 8.0f, 180.0f);
            gl.Color(0.329412, 0.329412, 0.329412);
            disk_holder.Render(gl, RenderMode.Render);
            gl.PopMatrix();


            gl.PushMatrix();
            Disk disk = new Disk();

            disk.InnerRadius = 2.0f;
            disk.OuterRadius = 10.0f;
            disk.CreateInContext(gl);
            gl.Translate(-330.0f + animationF3X, 290.0f + animationF3Y - animationF4Y, 180.0f + animationF1Z);
            gl.Scale(7.3f, 7.3f, 7.3f);
            gl.Rotate(0.0f, animationF2R, 0.0f);
            gl.Rotate(90.0f, 180.0f, 0.0f);

            gl.Color(255.0f, 255.0f, 255.0f); 
            disk.Render(gl, RenderMode.Render);
            gl.PopMatrix();
        }

        private void drawComputer(OpenGL gl)
        {
            drawComputerModel(gl);
            drawDiskAndHolder(gl);

        }


        public void SetupTexture(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);

            gl.GenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                // Pridruzi teksturu odgovarajucem identifikatoru
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);

                // Ucitaj sliku i podesi parametre teksture
                Bitmap image = new Bitmap(m_textureFiles[i]);
                // rotiramo sliku zbog koordinantog sistema opengl-a
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                // RGBA format (dozvoljena providnost slike tj. alfa kanal)
                BitmapData imageData = image.LockBits(rect, ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, imageData.Scan0);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST);		// Nearest neighbour filtering
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);		// Nearest neighbour filtering

                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);

                image.UnlockBits(imageData);
                image.Dispose();
            }
        }
        public void SetupLightRed(OpenGL gl)
        {
            float[] global_ambient = new float[] { 0.4f, 0.4f, 0.4f, 1.0f };
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, global_ambient);

            float[] light1pos = new float[] { 0.0f, 4000.0f, 0.0f, 1.0f };
            float[] light1ambient = new float[] { color_r, color_g, color_b, 1.0f };
            float[] light1diffuse = new float[] { 0.99f, 0.0f, 0.0f, 1.0f };
            float[] light1specular = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, light1pos);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, light1ambient);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, light1diffuse);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, light1specular);

            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT1);

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 30.0f);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, new float[] { 0.0f, -1.0f, 0.0f });

            // Ukljuci automatsku normalizaciju nad normalama
            gl.Enable(OpenGL.GL_NORMALIZE);

            gl.ShadeModel(OpenGL.GL_SMOOTH);
        }
        public void SetupLight(OpenGL gl)
        {
            float[] global_ambient = new float[] { 0.4f, 0.4f, 0.4f, 1.0f };
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, global_ambient);

            float[] light_pos = new float[] { 0.0f, 4000.0f, 0.0f, 1.0f };
            float[] light_ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] light_diffuse = new float[] { 0.99f, 0.99f, 0.99f, 1.0f };
            float[] light_specular = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light_pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light_ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light_diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light_specular);
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);

            // Ukljuci automatsku normalizaciju nad normalama
            gl.Enable(OpenGL.GL_NORMALIZE);
        }

        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;

            // prvi task 
            gl.Viewport(0, 0, m_width, m_height);

            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(45f, (double)width / (double)height, 1f, 10000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene.Dispose();
            }
        }

        public void changeReflector()
        {
            SetupLightRed(this.gl);
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
