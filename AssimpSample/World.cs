// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;
using SharpGL.Enumerations;
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

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 0f, 0f);
            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_FLAT);
            //ukljuceno testiranje dubine i skrivanje nevidljviih povrsina
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            m_scene.LoadScene();
            m_scene.Initialize();
        }





        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>

        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.FrontFace(OpenGL.GL_CCW);
            gl.Viewport(0, 0, m_width, m_height);

            gl.PushMatrix();

            gl.Translate(0.0f, 0.0f, -m_sceneDistance);

            gl.LookAt(0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, -1.0, 1.0);

            gl.Rotate( 0.0f, 225.0f, 0.0f);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);


            gl.PushMatrix();


            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(0.0f, 1.0f, 0.0f);
            gl.Color(0.71, 0.73, 0.75);

            //Crtanje podloge u smeru CCW (podrazumevano), da se vidi od gore
            gl.Vertex(2500.0f, 0.0, -2000.0f);
            gl.Vertex(-2500.0f, 0.0f, -2000.0f);
            gl.Vertex(-2500.0f, 0.0f, 2000.0f);
            gl.Vertex(2500.0f, 0.0f, 2000.0f);

            gl.End();

            gl.PopMatrix();

            gl.PushMatrix();


            Cube table = new Cube();

                gl.PushMatrix();

                gl.Translate(0.0f, 402.0f, -20.0f);
                gl.Scale(800.0f, 400.0f, 400.0f);
                gl.Color(0.30f, 0.15f, 0.00f);

                table.Render(gl, RenderMode.Render);

                gl.PopMatrix();

                gl.PushMatrix();

                gl.Translate(0.0f, 815.0f, 0.0f);
                gl.Scale(100.0f, 100.0f, 100.0f);  

                m_scene.Draw();

                gl.PopMatrix();

                gl.PushMatrix();

                Disk disk = new Disk();
                disk.InnerRadius = 2.0f;
                disk.OuterRadius = 10.0f;
                disk.CreateInContext(gl);
                gl.Translate(350.0f, 815.0f, 0.0f);
                gl.Scale(8.0f, 8.0f, 8.0f);
                gl.Rotate(90.0f, 180.0f, 0.0f);
                gl.Color(255.0f, 255.0f, 255.0f);
                disk.Render(gl, RenderMode.Render);

                gl.PopMatrix();

            gl.PopMatrix();

            gl.PopMatrix();

            gl.PushMatrix();
                gl.Viewport(m_width- 145, 0, m_width, m_height);
                gl.DrawText(0, 72, 255.0f, 255.0f, 0.0f, "Tahoma I talic", 10, "Predmet: Racunarska grafika");
                gl.DrawText(0, 72, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "_______________________");
                gl.DrawText(0, 59, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Sk.god: 2020/21.");
                gl.DrawText(0, 59, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "_____________");
                gl.DrawText(0, 46, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Ime: Aleksandar");
                gl.DrawText(0, 46, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "________");
                gl.DrawText(0, 33, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Prezime: Ignjatijevic");
                gl.DrawText(0, 33, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "________________");
                gl.DrawText(0, 20, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "Sifra zad: 6.2");
                gl.DrawText(0, 20, 255.0f, 255.0f, 0.0f, "Tahoma", 10, "____________");

            gl.PopMatrix();
            gl.Flush();
        }

        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;

            // prvi task skoro skroz uradjen
            gl.Viewport(0, 0, m_width, m_height);

            //TODO: explore how to change zFar atribute if needed
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(45f, (double)width / (double)height, 1f, 20000f);
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
