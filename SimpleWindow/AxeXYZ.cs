using OpenTK.Graphics.OpenGL;

using System.Drawing;

namespace SimpleWindow
{
    /// <summary>
    /// Această clasă desenează axele XYZ pentru o scenă 3D
    /// </summary>
    internal class AxeXYZ
    {
        private bool isHidden;

        private const int LUNGIME_AXE = 75;

        /// <summary>
        /// Constructor
        /// </summary>
        public AxeXYZ()
        {
            isHidden = false;
        }

        /// <summary>
        /// Metoda desenează axele
        /// </summary>
        public void Draw()
        {
            if (!isHidden)
            {
                GL.LineWidth(2);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Red);
                GL.Vertex3(-LUNGIME_AXE, 0, 0);
                GL.Vertex3(LUNGIME_AXE, 0, 0);

                GL.Color3(Color.Black);
                GL.Vertex3(0, -LUNGIME_AXE, 0);
                GL.Vertex3(0, LUNGIME_AXE, 0);

                GL.Color3(Color.Blue);
                GL.Vertex3(0, 0, -LUNGIME_AXE);
                GL.Vertex3(0, 0, LUNGIME_AXE);

                GL.End();
            }
        }

        /// <summary>
        /// Afișează axele
        /// </summary>
        public void Show()
        {
            isHidden = false;
        }

        /// <summary>
        /// Ascunde axele
        /// </summary>
        public void Hide()
        {
            isHidden = true;
        }

        /// <summary>
        /// Schimbă starea de afișare a obiectului
        /// </summary>
        public void ToggleVisibility()
        {
            isHidden = !isHidden;
        }
    }
}

