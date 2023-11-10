using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SimpleWindow
{
    /// <summary>
    /// Aceasta clasa gestioneaza controlul camerei si face parte din tema 3
    /// Camera poate fi controlata prin miscarea mouse-ului si prin apasarea left-click sau right-click.
    ///
    /// </summary>
    internal class Camera3D
    {
        private Vector3 eye = new Vector3(0, 10, 75);
        private Vector3 target = new Vector3(0, 0, 0);
        private Vector3 up = new Vector3(0, 1, 0);
        private const int distantaMaxima = 75;
        private const int incrementMiscare = 1;

        public void SetCamera()
        {
            Matrix4 camera = Matrix4.LookAt(eye, target, up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref camera);
        }
        public void MouseControl(bool enable,bool moveLeft,bool moveRight,bool moveUp, bool moveDown) // setereaza camera la pozitia mouseului
        {
            float curentX;
            float curentY;
            Matrix4 camera;
            if (enable)
            {
                if (moveLeft)
                {
                    curentX = Math.Max(eye.X - incrementMiscare, -distantaMaxima);
                    eye = new Vector3(curentX, eye.Y, eye.Z);
                    camera = Matrix4.LookAt(eye, target, up);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref camera);
                }
                if (moveRight)
                {
                    curentX = Math.Min(eye.X + incrementMiscare, distantaMaxima);
                    eye = new Vector3(curentX, eye.Y, eye.Z);
                    camera = Matrix4.LookAt(eye, target, up);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref camera);
                }
                if (moveUp)
                {
                    curentY = Math.Min(eye.Y + incrementMiscare, distantaMaxima);
                    eye = new Vector3(eye.X, curentY, eye.Z);
                    camera = Matrix4.LookAt(eye, target, up);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref camera);

                }
                if (moveDown)
                {
                    curentY = Math.Max(eye.Y - incrementMiscare, -distantaMaxima);
                    eye = new Vector3(eye.X, curentY, eye.Z);
                    camera = Matrix4.LookAt(eye, target, up);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref camera);

                }
            }


        }
        public void RotesteDreapta()
        {
            if (eye.X < distantaMaxima && eye.Z >= distantaMaxima)
            {
                eye = new Vector3(eye.X + incrementMiscare, eye.Y, eye.Z);
            }
            else if (eye.X >= distantaMaxima && eye.Z > -distantaMaxima)
            {
                eye = new Vector3(eye.X, eye.Y, eye.Z - incrementMiscare);
            }
            else if (eye.X > -distantaMaxima && eye.Z <= distantaMaxima)
            {
                eye = new Vector3(eye.X - incrementMiscare, eye.Y, eye.Z);
            }
            else
            {
                eye = new Vector3(eye.X, eye.Y, eye.Z + incrementMiscare);
            }
            
            SetCamera();
        }
        public void RotesteStanga()
        {
            if (eye.X > -distantaMaxima && eye.Z >= distantaMaxima)
            {
                eye = new Vector3(eye.X - incrementMiscare, eye.Y, eye.Z);
            }
            else if (eye.X <= -distantaMaxima && eye.Z > -distantaMaxima)
            {
                eye = new Vector3(eye.X, eye.Y, eye.Z - incrementMiscare);
            }
            else if (eye.X < distantaMaxima && eye.Z <= -distantaMaxima)
            {
                eye = new Vector3(eye.X + incrementMiscare, eye.Y, eye.Z);
            }
            else
            {
                eye = new Vector3(eye.X, eye.Y, eye.Z + incrementMiscare);
            }
            SetCamera();
        }

        public void ControlCamera(MouseState mouse,bool enable,bool isMoving,bool left,bool right, bool up,bool down)
        {
            if (isMoving)
            {
                MouseControl(enable,left,right,up,down); // controlul camerei din mouse
            }
            if (mouse[MouseButton.Left])
            {
                this.RotesteStanga();
            }
            else if (mouse[MouseButton.Right] )
            {
                this.RotesteDreapta();
            }
            
        }
    }
}
