using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWindow
{
    /// <summary>
    ///  Ilculesei-Meglei Ștefan grupa 3131a
    ///  Acest proiect rezolvă tema de la laboratorul 2 si din laboratorul 3.
    /// </summary>
    class SimpleWindow : GameWindow
    {

        /// <value>
        /// Câmpul <c>caleFisierdCoordonate</c> reprezintă calea catre fisierul de unde vom luat coordonatele obiectului, face parte din tema 3
        /// </value>
        string caleFisierCoordonate = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, "coordonate.txt");
        /// <value>
        /// Câmpul <c>caleFisierMeniu</c> reprezintă calea catre fisierul de unde vom luat meniul de ajutor
        /// </value>
        string caleFisierMeniu = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, "meniu.txt");
        /// <value>
        /// Câmpul <c> coordObj</c> reprezintă un array cu coordonatele obiectului, face parte din tema 3
        /// </value>
        private float[][] coordObj= new float[3][];
        /// <value>
        /// Câmpul <c> XYZ_SIZE</c> determina lungimea axelor de coordonate
        /// </value>
        public const int XYZ_SIZE = 75;
        /// <value>
        /// Câmpul <c> incrementCuloare</c> determina cu cat vom incrementa/decrementa culoarea obiectului, face parte din tema 3
        /// </value>
        private static int incrementCuloare = 20;
        /// <value>
        /// Câmpul <c> mouseMovement</c> determina daca camere este manipulata prin miscarea mouse-ului, face parte din tema 3
        /// </value>
        private bool mouseMovement = true;
        /// <value>
        /// Câmpul <c> camera</c> este folosit pentru a apela metodele din clasa Camare3D, face parte din tema3
        /// </value>
        private Camera3D camera;
        /// <value>
        /// Câmpul <c>previousKeyboard</c> reprezintă ultima stare a tastaturii
        /// </value>
        private KeyboardState previousKeyboard;
        /// <value>
        /// Câmpul <c>previousMouse</c> reprezintă ultima stare a mouse-ului
        /// </value>
        private MouseState previousMouse;
        /// <value>
        /// Câmpul <c>colorRest</c> este static și reprezintă un tablou pentru colorile obiectului în stare de repaus
        /// </value>
        private Color[] colorRest = new Color[3];
        /// <value>
        /// Câmpul <c>colorRest</c> este static și reprezintă un tablou pentru colorile obiectului în stare de mișcare
        /// </value>
        private Color[] colorMove = { Color.Red,Color.Yellow,Color.Blue};
        /// <value>
        /// Câmpul <c>constDeplasaret</c> este static și reprezintă viteza cu care se deplasează obiectul
        /// </value>
        static float constDeplasare = 0.02f;
        /// <value>
        /// Câmpul <c>X</c> reprezintă translația pe axa x a obiectului
        /// </value>
        private float X=0f;
        /// <value>
        /// Câmpul <c>Y</c> reprezintă translația pe axa y a obiectului
        /// </value>
        private float Y= 0f;
        /// <value>
        /// Câmpul <c>Z</c> reprezintă translația pe axa z a obiectului
        /// </value>
        private float Z = 0f;
        /// <value>
        /// Câmpul <c>isMoving</c> reprezintă starea de mișcare a mouse-ului
        /// </value>
        private bool isMoving=false;
        /// <value>
        /// Câmpul <c>isHidden</c> determină dacă obiectul este randat sau nu
        /// </value>
        private bool isHidden=false;
        /// <value>
        /// Câmpul <c>meniuExists</c> determină daca meniul a fost citit din fisier sau nu
        /// </value>
        bool meniuExists = false;
        /// <value>
        /// Câmpul <c>meniu</c> contine meniul de ajutor
        /// </value>
        private StringBuilder meniu = new StringBuilder();
        /// <summary>
        /// Constructor 
        /// </summary>
        public SimpleWindow() : base(800, 600,new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On; // se activează VSync
            KeyDown += Keyboard_KeyDown;
            camera = new Camera3D();
            initCoord();
            initCulori(); 
            displayHelp();

        }
        /// <summary>
        /// Metoda de initializare a culorilor pentru un obiect, seteaza o culoare random si face parte din tema 3
        /// </summary>
        private void initCulori() // da valori random pentru culorile obiectului in repus
        {
            Random rand=   new Random();
            for(int i = 0; i < colorRest.Length; i++) {
                colorRest[i] = Color.FromArgb(255,rand.Next(256),rand.Next(256),rand.Next(256));
            }

        }
        /// <summary>
        /// Metoda de initializare a coordonatelor pentru un obiect, coordonatele sunt citite dintr-un fisier si face parte din tema 3
        /// Sunt arunca exceptii in cazul in care fisierul nu exista, numarul de linii este gresit sau nu sunt 3 numere pe o linie
        /// </summary>
        private void initCoord()
        {
            for (int i = 0; i < 3; i++) { coordObj[i] = new float[3]; }
            try // citire din fisier
            {
                string[] linii= File.ReadAllLines(caleFisierCoordonate);
                if(linii.Length != 3) {
                    throw new Exception("numar incorect de linii");
                }
                for(int i = 0;i < linii.Length;i++)
                {
                    string[] sir = linii[i].Split(','); // separare dupa ,
                    if(sir.Length != 3) {
                        throw new Exception("numar incorect de coordonate pe linie");
                    }
                    coordObj[i][0] = float.Parse(sir[0]);
                    coordObj[i][1]= float.Parse(sir[1]);
                    coordObj[i][2]= float.Parse(sir[2]);

                }
                
            }
            catch (Exception e) { Console.WriteLine("Exceptie:"+e.Message); }
        }

        /// <summary>
        /// Tratează evenimentul generat de apăsarea unui taste. 
        /// Sunt tratate tastele :
        /// Escape - iese din aplicatie, 
        /// F11 - redimensioneaza fereastra,
        /// H -  afiseaza meniul de ajutor,
        /// O - ascunde/arata obiectul (face parte din tema 2),
        /// leftArrow - misca obiectul in staga si trebuie tinut apasat (face parte din tema 2),
        /// rightArro - misca obiectul in dreata si trebuie tinut apasat (face parte din tema 2),
        /// UpArrow - misca obiectul in sus si trebuie tinut apasat,
        /// DownArrow - misca obiectul in joc si trebuie tinut apasat,
        /// Control + UpArrow - mica obiectul in fata si trebuie tinut apasat,
        /// Control + DownArrow - mica obiectul in spate si trebuie tinut apasat,
        /// Space - centreaza obiectul,
        /// R - creste componenta Red din culorile obiectului (face parte din tema 3),
        /// Control + R - scade componenta Red din culorile obiectului (face parte din tema 3),
        /// G - creste componenta Green din culorile obiectului (face parte din tema 3),
        /// Control + G - scade componenta Green din culorile obiectului (face parte din tema 3),
        /// B - creste componenta Blue din culorile obiectului (face parte din tema 3),
        /// Control + B - scade componenta Blue din culorile obiectului (face parte din tema 3).
        /// X - seteaza daca camera este manipulata prin miscarea mouse-ului sau nu (face parte din tema 3),
        /// I - informatii despre obiect momentan doar culorile in format RGB (face parte din tema 3)
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
                this.isHidden =!isHidden; // se ascunde/arata obiectul
            }
            if (e.Key == Key.X && !previousKeyboard[Key.X]) // se verifică dacă tasta x a fost apăsata o singură dată
            {
                this.mouseMovement = !mouseMovement; // se seteaza controlul camerei cu mouse-ul
            }
            if (e.Key == Key.Left && previousKeyboard[Key.Left]) // se verifică dacă este apăsată tasta leftArrow
            {
                X -= constDeplasare;
            }
            if (e.Key == Key.Right && previousKeyboard[Key.Right]) // se verifică dacă este apăsată tasta rightArrow
            {
               
                X += constDeplasare;
            }
            if (e.Key == Key.Up && previousKeyboard[Key.Up]) // se verifică dacă este apăsată tasta UpArrow
            {
                Y += constDeplasare;
            }
            if (e.Key == Key.Down && previousKeyboard[Key.Down]) // se verifică dacă este apăsată tasta DownArrow
            {
                Y -= constDeplasare;
            }
            if (e.Key == Key.Up && previousKeyboard[Key.Up] && e.Control) // se verifică dacă este apăsată tasta UpArrow si Control
            {
                Z += constDeplasare;
            }
            if (e.Key == Key.Down && previousKeyboard[Key.Down] && e.Control) // se verifică dacă este apăsată tasta DownArrow si Control
            {
                Z -= constDeplasare;
            }
            if (e.Key == Key.Space) // se verifică dacă este apăsată tasta Space
            {
                X = 0f;
                Y = 0f;
                Z = 0f;
            }

            if (e.Key == Key.R && !previousKeyboard[Key.R] && !e.Control) // se verifică dacă este apăsată tasta R
            {

                for (int i = 0; i < colorRest.Length; i++)
                {
                    int culoare = Math.Min(255, colorRest[i].R + incrementCuloare);
                    colorRest[i] = Color.FromArgb(culoare, colorRest[i].G, colorRest[i].B);
                
                }

            }
            if (e.Key == Key.R && !previousKeyboard[Key.R] && e.Control) // se verifică dacă este apăsată tasta R si Control
            {

                for (int i = 0; i < colorRest.Length; i++)
                {
                    int culoare = Math.Max(0, colorRest[i].R - incrementCuloare);
                    colorRest[i] = Color.FromArgb(culoare, colorRest[i].G, colorRest[i].B);
               
                }
            }
            if (e.Key == Key.G && !previousKeyboard[Key.G] && !e.Control) // se verifică dacă este apăsată tasta G
            {
         
                for (int i = 0; i < colorRest.Length; i++)
                {
                    int culoare = Math.Min(255, colorRest[i].G + incrementCuloare);
                    colorRest[i] = Color.FromArgb(colorRest[i].R, culoare, colorRest[i].B);
                  
                 }
           
            }
            if (e.Key == Key.G && !previousKeyboard[Key.G] && e.Control) // se verifică dacă este apăsată tasta G si Control
            {
          
                for (int i = 0; i < colorRest.Length; i++)
                {
                    int culoare = Math.Max(0, colorRest[i].G - incrementCuloare);
                    colorRest[i] = Color.FromArgb(colorRest[i].R, culoare, colorRest[i].B);
                   
                }
             
            }
            if (e.Key == Key.B && !previousKeyboard[Key.B] && !e.Control) // se verifică dacă este apăsată tasta B
            {
         
                for (int i = 0; i < colorRest.Length; i++)
                {
                    int culoare = Math.Min(255, colorRest[i].B + incrementCuloare);
                    colorRest[i] = Color.FromArgb(colorRest[i].R, colorRest[i].G, culoare);
                  
                }
          
            }
            if (e.Key == Key.B && !previousKeyboard[Key.B] && e.Control) // se verifică dacă este apăsată tasta R si Control
            {
                
                for (int i = 0; i < colorRest.Length; i++)
                {
                    int culoare = Math.Max(0, colorRest[i].B - incrementCuloare);
                    colorRest[i] = Color.FromArgb(colorRest[i].R, colorRest[i].G, culoare);
                 
                }
             
            }
            if (e.Key == Key.I && !previousKeyboard[Key.I]) // se verifica daca este apasata tasta I
            {
                Console.WriteLine("Informatii despre obiectul in repaus:");
                for (int i = 0; i < colorRest.Length; i++)
                {
                    Console.WriteLine("Vertex[" + i + "] are valorile RGB:{" + colorRest[i].R + "," + colorRest[i].G + "," + colorRest[i].B + "}");

                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Setarea mediului OpenGL și a culorii fundalului ferestrei 3D
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
            GL.Viewport(0, 0, this.Width, this.Height);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 256);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

        }

        
        /// <summary>
        /// Secțiunea pentru "game logic"/"business logic". Tot ce se execută în această secțiune va fi randat
        /// automat pe ecran în pasul următor.
        /// Actualizează tasta apăsată și starea mouse-ului, verifică dacă mouse-ul se deplasează.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Variabilele left,right,up,down fac parte din tema3 si determina controlul camerei
            bool left=false;
            bool right=false;
            bool up = false;
            bool down = false;
            // Proprietățile X și Y reprezintă poziția absolută a mouse-ului
            if((previousMouse.X != Mouse.GetState().X) || (previousMouse.Y != Mouse.GetState().Y)) // se verifică dacă mouse-ul se deplasează
            {
                isMoving = true;
                // urmeaza setarea variabilelor left,right,up,down in functie de miscarea mouse-ului
                if (previousMouse.X<Mouse.GetState().X) 
                {
                    right= true;
                    left = false;
                }
                else
                    if(previousMouse.X > Mouse.GetState().X)
                {
                    left= true;
                    right= false;
                }

                if(previousMouse.Y < Mouse.GetState().Y)
                {
                    up= true;
                    down= false;
                }
                else
                    if(previousMouse.Y > Mouse.GetState().Y)
                {
                    down= true;
                    up= false;
                }
            }
            else { isMoving = false; }


            this.previousMouse = Mouse.GetState(); // se actualizează starea mouse-ului
            this.previousKeyboard = Keyboard.GetState(); // se actualizează starea tastaturii
            // se apeleaza controlul camerei, mouseMovement determina daca miscarea mouse-ului afecteaza camera
            camera.ControlCamera(previousMouse,mouseMovement,isMoving,left,right,up,down); 
        }

        /// <summary>
        /// Secțiunea pentru randarea scenei 3D. Controlată de modulul logic din metoda ONUPDATEFRAME.
        /// Randează obiectul în funcție de stările logice ale câmpurilor <c>isMoving</c> și <c>isHidden</c>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
           
            GL.Clear(ClearBufferMask.ColorBufferBit);
            DrawAxes(); // se deseaneaza axele
            
            if (!isHidden) // verificăm dacă obiectul nu este ascuns
            {
                if (!isMoving) // verificăm dacă mouse-ul este în mișcare
                {
                    // se deseneaza triunghiul stationar, el va avea culoarea random la inceput si poate fi schimbata cu tastele R,G,B si Control + aceleasi taste
                    GL.Begin(PrimitiveType.Triangles);

                    GL.Color3(colorRest[0]);
                    GL.Vertex3(coordObj[0][0]+X, coordObj[0][1]+Y, coordObj[0][2]+Z);
                    GL.Color3(colorRest[1]);
                    GL.Vertex3(coordObj[1][0]+X, coordObj[1][1]+Y, coordObj[1][2]+Z);
                    GL.Color3(colorRest[2]);
                    GL.Vertex3(coordObj[2][0] + X, coordObj[2][1] + Y, coordObj[2][2] + Z);

                    GL.End();
                }
                else
                {
                    // se deseneaza obiectul in miscare, el va fi tot un triunghi, dar va avea culori statice
                    GL.Begin(PrimitiveType.Triangles);

                    GL.Color3(colorMove[0]);
                    GL.Vertex3(coordObj[0][0] + X, coordObj[0][1] + Y, coordObj[0][2] + Z);
                    GL.Color3(colorMove[1]);
                    GL.Vertex3(coordObj[1][0]+X, coordObj[1][1] + Y, coordObj[1][2] + Z);
                    GL.Color3(colorMove[2]);
                    GL.Vertex3(coordObj[2][0] + X, coordObj[2][1] + Y, coordObj[2][2] + Z);
                    GL.End();

                }
            }
            this.SwapBuffers();
        }
        /// <summary>
        /// Metoda care deseaneaza axele 
        /// </summary>
        private void DrawAxes()

        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Black);
            GL.Vertex3(-XYZ_SIZE, 0, 0);
            GL.Vertex3(XYZ_SIZE, 0, 0);

            GL.Color3(Color.Black);
            GL.Vertex3(0, -XYZ_SIZE, 0);
            GL.Vertex3(0, XYZ_SIZE, 0);
         
            GL.Color3(Color.Black);
            GL.Vertex3(0, 0, -XYZ_SIZE);
            GL.Vertex3(0, 0, XYZ_SIZE);

            GL.End();
        }


        /// <summary>
        /// Metodă care afișează în consolă meniul aplicației.
        /// </summary>

        private void displayHelp()
        {
            if(!meniuExists) // verificam daca meniul a fost citit sau nu
            {
                try // citire din fisier
                {
                    string[] linii = File.ReadAllLines(caleFisierMeniu);
                    for (int i = 0; i < linii.Length; i++)
                    {
                        meniu.Append(linii[i]+"\n");
                    }
                    meniu.Append("\n");

                }
                catch (Exception e) { Console.WriteLine("Exceptie:" + e.Message); }
                

            }
            Console.WriteLine(meniu); 
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
