using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWindow
{
    /// <summary>
    ///  Ilculesei-Meglei Ștefan grupa 3131a
    ///  Acest proiect rezolvă tema de la laboratorul 2.
    /// </summary>
    class SimpleWindow : GameWindow
    {
        /// <value>
        /// Câmpul <c>previousKeyboard</c> reprezintă ultima stare a tastaturii
        /// </value>
        KeyboardState previousKeyboard;
        /// <value>
        /// Câmpul <c>previousMouse</c> reprezintă ultima stare a mouse-ului
        /// </value>
        MouseState previousMouse;
        /// <value>
        /// Câmpul <c>colorRest</c> este static și reprezintă un tablou pentru colorile obiectului în stare de repaus
        /// </value>
        static Color[] colorRest = { Color.MidnightBlue, Color.SpringGreen, Color.Ivory };
        /// <value>
        /// Câmpul <c>colorRest</c> este static și reprezintă un tablou pentru colorile obiectului în stare de mișcare
        /// </value>
        static Color[] colorMove = { Color.Red,Color.Yellow,Color.Blue};
        /// <value>
        /// Câmpul <c>isMoving</c> reprezintă starea de mișcare a mouse-ului
        /// </value>
        bool isMoving;
        /// <value>
        /// Câmpul <c>isHidden</c> determină dacă obiectul este randat sau nu
        /// </value>
        bool isHidden;
        /// <summary>
        /// Constructor 
        /// </summary>
        public SimpleWindow() : base(800, 600,new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On; // se activează VSync
            KeyDown += Keyboard_KeyDown;
            displayHelp();
            isMoving = false;
            isHidden = false;
        }

     
        /// <summary>
        /// Tratează evenimentul generat de apăsarea unui taste. 
        /// Sunt tratate tastele Escape, F11, H, O și P.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape) // se verifică dacă este apăsată tasta Esc
            {
                this.Exit();
            }
            if (e.Key == Key.F11 && !previousKeyboard[Key.F11]) // se verifică dacă tasta F11 a fost apăsata o singură dată
            {
                if (this.WindowState == WindowState.Fullscreen) // se verifică dacă modul curent este Fullscreen
                {
                    this.WindowState = WindowState.Normal; // modul ferestrei devine normal
                }
                else
                {
                    this.WindowState = WindowState.Fullscreen; // modul ferestrei devine Fullscreen
                }
            }
            if (e.Key == Key.H && !previousKeyboard[Key.H]) // se verifică dacă tasta H a fost apăsata o singură dată
            {
                displayHelp(); // se afișează meniul în consolă
            }

            if (e.Key == Key.O && !previousKeyboard[Key.O]) // se verifică dacă tasta O a fost apăsata o singură dată
            {
                this.isHidden = true; // se ascunde obiectul
            }

            if(e.Key == Key.P && !previousKeyboard[Key.P]) // se verifică dacă tasta P a fost apăsata o singură dată
            {
                this.isHidden= false; // se afisează obiectul
            }

        }

        /// <summary>
        /// Setarea mediului OpenGL și a culorii fundalului ferestrei 3D
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.DarkGray); // se setează culoarea de fundal
        }

        /// <summary>
        /// Inițierea afișării și setarea viewport-ului grafic. Metoda este invocată la redimensionarea
        /// ferestrei. Va fi invocată o dată și imediat după metoda ONLOAD!
        /// Viewport-ul va fi dimensionat conform mărimii ferestrei active.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height); 

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        
        /// <summary>
        /// Secțiunea pentru "game logic"/"business logic". Tot ce se execută în această secțiune va fi randat
        /// automat pe ecran în pasul următor.
        /// Actualizează tasta apăsată și starea mouse-ului, verifică dacă mouse-ul se deplasează.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Proprietățile X și Y reprezintă poziția absolută a mouse-ului
            if((previousMouse.X != Mouse.GetState().X) || (previousMouse.Y != Mouse.GetState().Y)) // se verifică dacă mouse-ul se deplasează
            {
                isMoving = true; 
            }
            else { isMoving = false; }
            this.previousMouse = Mouse.GetState(); // se actualizează starea mouse-ului
            this.previousKeyboard = Keyboard.GetState(); // se actualizează starea tastaturii
        }

        /// <summary>
        /// Secțiunea pentru randarea scenei 3D. Controlată de modulul logic din metoda ONUPDATEFRAME.
        /// Randează obiectul în funcție de stările logice ale câmpurilor <c>isMoving</c> și <c>isHidden</c>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

          
            if (!isHidden) // verificăm dacă obiectul nu este ascuns
            {
                if (!isMoving) // verificăm dacă mouse-ul este în mișcare
                {
                    GL.Begin(PrimitiveType.Triangles);

                    GL.Color3(colorRest[0]);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.Color3(colorRest[1]);
                    GL.Vertex2(0.0f, -1.0f);
                    GL.Color3(colorRest[2]);
                    GL.Vertex2(1.0f, 1.0f);

                    GL.End();
                }
                else
                {
                    GL.Begin(PrimitiveType.Triangles);

                    GL.Color3(colorMove[0]);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.Color3(colorMove[1]);
                    GL.Vertex2(0.0f, -1.0f);
                    GL.Color3(colorMove[2]);
                    GL.Vertex2(1.0f, 1.0f);

                    GL.End();

                }
            }
            this.SwapBuffers();
        }



        /// <summary>
        /// Metodă care afișează în consolă meniul aplicației.
        /// </summary>

        private void displayHelp()
        {
            Console.WriteLine("\n         MENIU");
            Console.WriteLine(" ESC - parasire aplicatie");
            Console.WriteLine(" H - meniu de ajutor");
            Console.WriteLine(" O - ascunde obiectul");
            Console.WriteLine(" P - arata obiectul");
            Console.WriteLine(" F11 - redimensionare fereastra");
        }

        [STAThread]
        static void Main(string[] args)
        {

            // Utilizând cuvântul cheie ”using” asigurăm o dealocare a memorie rapidă.
            // Metoda ”Run()” specifică numărul evenimente de tip UpdateFrame pe secundă, 30 în acest caz, și un număr nelimitat de evenimente
            // de tip randare.
            // În principiu având numărul de update-uri pe secundă la 30, FPS-ul maxim pentru această aplicație este tot 30. Dacă se randează mai multe
            // frame-uri pe secundă acestea vor fi duplicate ale scenei anterioare.
            using (SimpleWindow window3D = new SimpleWindow())
            {
                window3D.Run(30.0, 0.0);
            }
        }
    }
}
