using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;


namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;

        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            // Inicijalizacija komponenti
            InitializeComponent();



            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Computer"), "Wolf Logan.3DS", (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight, openGLControl.OpenGL);

            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }
        }

        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!m_world.isAnimation) 
            { 
                switch (e.Key)
                {
                    case Key.F4: this.Close(); break;
                        //potrebno mozda menjati  jer se sad okrece ovake zbog trenutnog cudnog ugla gledanja
                    case Key.W: 
                        
                        if(m_world.RotationX <= -90.0f)
                        {
                            break;
                        }
                        
                        m_world.RotationX -= 5.0f; 
                        break;
                    case Key.S:
                        
                        if (m_world.RotationX >= 20.0f)
                        {
                            break;
                        }
                        
                        m_world.RotationX += 5.0f;
                        break;
                    case Key.A: m_world.RotationY -= 5.0f; break;
                    case Key.D: m_world.RotationY += 5.0f; break;
                    case Key.Add: m_world.SceneDistance -= 700.0f; break;
                    case Key.Subtract: m_world.SceneDistance += 700.0f; break;
                    case Key.C:
                        m_world.isAnimation = true;
                        
                        m_world.timer.Start();
                        break;
                    case Key.F2:
                        OpenFileDialog opfModel = new OpenFileDialog();
                        bool result = (bool)opfModel.ShowDialog();
                        if (result)
                        {

                            try
                            {
                                World newWorld = new World(Directory.GetParent(opfModel.FileName).ToString(), Path.GetFileName(opfModel.FileName), (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
                                m_world.Dispose();
                                m_world = newWorld;
                                m_world.Initialize(openGLControl.OpenGL);
                            }
                            catch (Exception exp)
                            {
                                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta:\n" + exp.Message, "GRESKA", MessageBoxButton.OK);
                            }
                        }
                        break;
                }
            }
            
        }
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!m_world.isAnimation)
            {
                if (this.slider1.Value == 0)
                {
                    this.slider1.Value = 0.1;
                    m_world.Redraw();
                }
                else
                {
                    m_world.scaleFactor = this.slider1.Value;
                    m_world.Redraw();
            
                }
            }
        }
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!m_world.isAnimation)
            {
                m_world.translateFactor = this.slider2.Value;
                m_world.Redraw();
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!m_world.isAnimation)
            {
                if (isEmpty(this.red.Text))
                {
                    m_world.color_r = 0;
                }
                else
                {
                    m_world.color_r = (float)Double.Parse(this.red.Text) / 255.0f;
                }

                if (isEmpty(this.green.Text))
                {
                    m_world.color_g = 0;
                }
                else
                {
                    m_world.color_g = (float)Double.Parse(this.green.Text) / 255.0f;
                }

                if (isEmpty(this.blue.Text))
                {
                    m_world.color_b = 0;
                }
                else
                {
                    m_world.color_b = (float)Double.Parse(this.blue.Text) / 255.0f;
                }


                m_world.changeReflector();
            }
        }
        private bool isEmpty(string str)
        {
            return str.Equals("");
        }
    }
}
