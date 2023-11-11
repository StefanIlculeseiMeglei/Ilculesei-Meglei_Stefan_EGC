using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SimpleWindow
{
    /// <summary>
    /// Aceasta clasa gestioneaza controlul camerei si face parte din Tema 3
    /// Camera poate fi controlata prin miscarea mouse-ului si prin apasarea tastelor A și D
    ///
    /// </summary>
    internal class Camera3D
    {
        private Vector3 eye = new Vector3(0, 10, 75);
        private Vector3 target = new Vector3(0, 0, 0);
        private Vector3 up = new Vector3(0, 1, 0);
        private const int distantaMaximaNear = 75;
        private const int distantaMaximaFar = 150;
        private const int incrementMiscareNear = 2;
        private const int incrementMiscareFar = 5;
        /// <summary>
        /// Câmpul determină dacă camera este aproape sau departe.
        /// </summary>
        private bool isNear = true;
        /// <summary>
        /// Metoda schimbă punctul de vizionare a camerei din poziția aproape în departe. Face parte din Tema 5(subpunct 2)
        /// </summary>
        public void ToggleCameraDistance()
        {
            isNear = !isNear;
            if (isNear)
                eye = new Vector3(0, 10, 75);
            else
                eye = new Vector3(0, 40, 200);
            SetCamera();
    }
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
                if (isNear)
                {
                    if (moveLeft)
                    {
                        curentX = Math.Max(eye.X - incrementMiscareNear, -distantaMaximaNear);
                        eye = new Vector3(curentX, eye.Y, eye.Z);
                        camera = Matrix4.LookAt(eye, target, up);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadMatrix(ref camera);
                    }
                    if (moveRight)
                    {
                        curentX = Math.Min(eye.X + incrementMiscareNear, distantaMaximaNear);
                        eye = new Vector3(curentX, eye.Y, eye.Z);
                        camera = Matrix4.LookAt(eye, target, up);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadMatrix(ref camera);
                    }
                    if (moveUp)
                    {
                        curentY = Math.Min(eye.Y + incrementMiscareNear, distantaMaximaNear);
                        eye = new Vector3(eye.X, curentY, eye.Z);
                        camera = Matrix4.LookAt(eye, target, up);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadMatrix(ref camera);

                    }
                    if (moveDown)
                    {
                        curentY = Math.Max(eye.Y - incrementMiscareNear, -distantaMaximaNear);
                        eye = new Vector3(eye.X, curentY, eye.Z);
                        camera = Matrix4.LookAt(eye, target, up);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadMatrix(ref camera);

                    }
                }
                else
                {
                    if (moveLeft)
                    {
                        curentX = Math.Max(eye.X - incrementMiscareFar, -distantaMaximaFar);
                        eye = new Vector3(curentX, eye.Y, eye.Z);
                        camera = Matrix4.LookAt(eye, target, up);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadMatrix(ref camera);
                    }
                    if (moveRight)
                    {
                        curentX = Math.Min(eye.X + incrementMiscareFar, distantaMaximaFar);
                        eye = new Vector3(curentX, eye.Y, eye.Z);
                        camera = Matrix4.LookAt(eye, target, up);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadMatrix(ref camera);
                    }
                    if (moveUp)
                    {
                        curentY = Math.Min(eye.Y + incrementMiscareFar, distantaMaximaFar);
                        eye = new Vector3(eye.X, curentY, eye.Z);
                        camera = Matrix4.LookAt(eye, target, up);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadMatrix(ref camera);

                    }
                    if (moveDown)
                    {
                        curentY = Math.Max(eye.Y - incrementMiscareFar, -distantaMaximaFar);
                        eye = new Vector3(eye.X, curentY, eye.Z);
                        camera = Matrix4.LookAt(eye, target, up);
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadMatrix(ref camera);

                    }

                }
            }


        }
        /// <summary>
        /// Metoda care roteste camera la dreapta.
        /// </summary>
        public void RotesteDreapta()
        {
            if (isNear)
            {
                if (eye.X < distantaMaximaNear && eye.Z >= distantaMaximaNear)
                {
                    eye = new Vector3(eye.X + incrementMiscareNear, eye.Y, eye.Z);
                }
                else if (eye.X >= distantaMaximaNear && eye.Z > -distantaMaximaNear)
                {
                    eye = new Vector3(eye.X, eye.Y, eye.Z - incrementMiscareNear);
                }
                else if (eye.X > -distantaMaximaNear && eye.Z <= distantaMaximaNear)
                {
                    eye = new Vector3(eye.X - incrementMiscareNear, eye.Y, eye.Z);
                }
                else
                {
                    eye = new Vector3(eye.X, eye.Y, eye.Z + incrementMiscareNear);
                }
            }
            else
            {
                if (eye.X < distantaMaximaFar && eye.Z >= distantaMaximaFar)
                {
                    eye = new Vector3(eye.X + incrementMiscareFar, eye.Y, eye.Z);
                }
                else if (eye.X >= distantaMaximaFar && eye.Z > -distantaMaximaFar)
                {
                    eye = new Vector3(eye.X, eye.Y, eye.Z - incrementMiscareFar);
                }
                else if (eye.X > -distantaMaximaFar && eye.Z <= distantaMaximaFar)
                {
                    eye = new Vector3(eye.X - incrementMiscareFar, eye.Y, eye.Z);
                }
                else
                {
                    eye = new Vector3(eye.X, eye.Y, eye.Z + incrementMiscareFar);
                }

            }
            
            SetCamera();
        }
        /// <summary>
        /// Metoda care rotește camera la stânga.
        /// </summary>
        public void RotesteStanga()
        {
            if (isNear)
            {
                if (eye.X > -distantaMaximaNear && eye.Z >= distantaMaximaNear)
                {
                    eye = new Vector3(eye.X - incrementMiscareNear, eye.Y, eye.Z);
                }
                else if (eye.X <= -distantaMaximaNear && eye.Z > -distantaMaximaNear)
                {
                    eye = new Vector3(eye.X, eye.Y, eye.Z - incrementMiscareNear);
                }
                else if (eye.X < distantaMaximaNear && eye.Z <= -distantaMaximaNear)
                {
                    eye = new Vector3(eye.X + incrementMiscareNear, eye.Y, eye.Z);
                }
                else
                {
                    eye = new Vector3(eye.X, eye.Y, eye.Z + incrementMiscareNear);
                }
            }
            else
            {
                if (eye.X > -distantaMaximaFar && eye.Z >= distantaMaximaFar)
                {
                    eye = new Vector3(eye.X - incrementMiscareFar, eye.Y, eye.Z);
                }
                else if (eye.X <= -distantaMaximaFar && eye.Z > -distantaMaximaFar)
                {
                    eye = new Vector3(eye.X, eye.Y, eye.Z - incrementMiscareFar);
                }
                else if (eye.X < distantaMaximaFar && eye.Z <= -distantaMaximaFar)
                {
                    eye = new Vector3(eye.X + incrementMiscareFar, eye.Y, eye.Z);
                }
                else
                {
                    eye = new Vector3(eye.X, eye.Y, eye.Z + incrementMiscareFar);
                }

            }
            SetCamera();
        }

        public void ControlCamera(bool enable,bool isMoving,bool left,bool right, bool up,bool down)
        {
            if (isMoving)
            {
                MouseControl(enable,left,right,up,down); // controlul camerei din mouse
            }
            
        }
    }
}
