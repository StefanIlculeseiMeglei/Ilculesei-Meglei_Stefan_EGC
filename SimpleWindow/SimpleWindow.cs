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
    ///  Acest proiect rezolvă Temele 2,3 și 4.
    /// </summary>
    class SimpleWindow : GameWindow
    {
        /// <value>
        /// Câmpul <c>caleFisierMeniu</c> reprezintă calea catre fisierul de unde vom luat meniul de ajutor
        /// </value>
        string caleFisierMeniu = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, "assets/meniu.txt");
        /// <value>
        /// Câmpul <c> XYZ_SIZE</c> determina lungimea axelor de coordonate
        /// </value>
        public const int XYZ_SIZE = 300;
        /// <value>
        /// Câmpul <c> incrementCuloare</c> determina cu cat vom incrementa/decrementa culoarea obiectului, face parte din tema 3
        /// </value>
        private static int incrementCuloare = 20;
        /// <summary>
        /// Câmp care determină numărul maxim de obiecte care poate pot fi desenate, face parte din Tema 5
        /// </summary>
        private int nrMaxObiecte = 10;
        /// <summary>
        /// Câmp care determină numărul curent de obiecte generate prin click, face parte din Tema 5
        /// </summary>
        private int nrCurentObiecte;
        /// <value>
        /// Câmpul <c> mouseMovement</c> determina daca camere este manipulata prin miscarea mouse-ului, face parte din tema 3
        /// </value>
        private bool mouseMovement = false;
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
        /// Câmpul <c>constDeplasaret</c> este static și reprezintă viteza cu care se deplasează obiectul
        /// </value>
        private static float constDeplasare = 1f;
       
        /// <value>
        /// Câmpul <c>isMoving</c> reprezintă starea de mișcare a mouse-ului
        /// </value>
        private bool isMoving=false;
   
        /// <value>
        /// Câmpul <c>meniuExists</c> determină daca meniul a fost citit din fisier sau nu
        /// </value>
        bool meniuExists = false;
        /// <value>
        /// Câmpul <c>meniu</c> contine meniul de ajutor
        /// </value>
        private StringBuilder meniu = new StringBuilder();
        /// <summary>
        /// Câmpul <c>obiect</c> reprezintă obiectul desenat
        /// </summary>
        private Obiect3D obiect;

        /// <summary>
        /// Câmpul <c>obiecte</c> reprezintă obiecte care vor fi generate prin click
        /// </summary>
        private Obiect3D[] obiecte;
        /// <summary>
        /// Câmpul <c>axe</c> reprezintă axele de coordonate a scenei 3D
        /// </summary>
        private AxeXYZ axe;
        /// <summary>
        /// Câmpul reprezintă gridul scenei.
        /// </summary>
        private Grid grid;
        /// <summary>
        /// Câmpul <c>modificaculori</c> determină ce obiecte îsi schimbă culoarea, face parte din Tema 4
        /// </summary>
        private ModificareCulori modificareCulori;
        /// <summary>
        /// Enumeratia ModificareCulori determină ce obiecte îsi vor schimba culoarea, face parte din Tema 4
        /// Fata - o singura fața a cubului îsi schimba culoarea
        /// Vertex1 - primul vertex din toate triunghiurile îsi schimbă culoarea
        /// Vertex2 - al doilea vertex din toate triunghiurile îsi schimbă culoarea
        /// Vertex3 - al treilea vertex din toate triunghiurile îsi schimbă culoarea
        /// </summary>
        internal enum ModificareCulori // TEMA 4
        {
            FATA,
            VERTEX1, VERTEX2, VERTEX3
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleWindow() : base(800, 600,new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On; // se activează VSync
            KeyDown += Keyboard_KeyDown;
            this.MouseDown +=this.OnMouseClick;
            camera = new Camera3D();
            obiect = new Obiect3D(); // obiectul principal pe care il vom manipula
            obiecte = new Obiect3D[nrMaxObiecte]; // array-ul de obiecte generate prin click face parte din Tema 5
            nrCurentObiecte = 0; // numarul curent de obiecte generate prin click
            axe= new AxeXYZ();
            grid = new Grid();
            modificareCulori = ModificareCulori.FATA;
            displayHelp();
            camera.SetCamera();
        }
        /// <summary>
        /// Metoda crează un nou obiect la apăsatea left click, face parte din Tema 5 (subpunct 1)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseClick(object sender,MouseEventArgs e)
        {
           if (e.Mouse[MouseButton.Left] && e.Mouse[MouseButton.Left] != previousMouse[MouseButton.Left])
           {
                if (nrCurentObiecte < nrMaxObiecte)
                {
                    obiecte[nrCurentObiecte] = new Obiect3D();
                    obiecte[nrCurentObiecte].TranslateRandom();
                    obiecte[nrCurentObiecte].ToggleVisibility();
                    obiecte[nrCurentObiecte].gravity = true;
                    nrCurentObiecte++;
                }
                else
                {
                    /// marim array-ul cu obiecte
                    nrMaxObiecte *= 2;
                    Obiect3D[] copieObiecte = new Obiect3D[nrMaxObiecte];
                    obiecte.CopyTo(copieObiecte, 0);
                    obiecte = copieObiecte;
                }
           }
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
        /// I - informatii despre obiect momentan doar culorile in format RGB si numarul de vertexuri (face parte din tema 3)
        /// 1 - schimba culorile primului vertex din toate triunghiurile
        /// 2 - schimba culorile celui de al doilea vertex din toate triunghiurile
        /// 3 - schimba culorile celui de al treilea vertex din toate triunghiurile
        /// 0 - schimba culorile unei fete a obiectului desenat]
        /// F1 - genereaza noi culorile pentru obiect
        /// F2 - Toggle Camara mode (near/far)
        /// S- salveaza noilea coordonate in fisier
        /// A- roteste camera la stanga
        /// D- roteste camera la dreapta
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
            if (e.Key == Key.F2 && !previousKeyboard[Key.F2]) // se verifică dacă tasta F2 a fost apăsata o singură dată
            {
                camera.ToggleCameraDistance();
            }

                if (e.Key == Key.H && !previousKeyboard[Key.H]) // se verifică dacă tasta H a fost apăsata o singură dată
            {
                displayHelp(); // se afișează meniul în consolă
            }

            if (e.Key == Key.O && !previousKeyboard[Key.O]) // se verifică dacă tasta O a fost apăsata o singură dată
            {
                obiect.ToggleVisibility(); // se ascunde/arata obiectul
            }
            if (e.Key == Key.X && !previousKeyboard[Key.X]) // se verifică dacă tasta x a fost apăsata o singură dată
            {
                this.mouseMovement = !mouseMovement; // se seteaza controlul camerei cu mouse-ul
            }
            if (e.Key == Key.Left && previousKeyboard[Key.Left]) // se verifică dacă este apăsată tasta leftArrow
            {
                obiect.Translate(-constDeplasare,0,0);
            }
            if (e.Key == Key.Right && previousKeyboard[Key.Right]) // se verifică dacă este apăsată tasta rightArrow
            {

                obiect.Translate(constDeplasare, 0, 0);
            }
            if (e.Key == Key.Up && previousKeyboard[Key.Up] && !e.Control) // se verifică dacă este apăsată tasta UpArrow
            {
                obiect.Translate(0, constDeplasare, 0);
            }
            if (e.Key == Key.Down && previousKeyboard[Key.Down] && !e.Control) // se verifică dacă este apăsată tasta DownArrow
            {
                obiect.Translate(0, -constDeplasare, 0);
            }
            if (e.Key == Key.Up && previousKeyboard[Key.Up] && e.Control) // se verifică dacă este apăsată tasta UpArrow si Control
            {
                obiect.Translate(0, 0, constDeplasare);
            }
            if (e.Key == Key.Down && previousKeyboard[Key.Down] && e.Control) // se verifică dacă este apăsată tasta DownArrow si Control
            {
                obiect.Translate(0, 0, -constDeplasare);
            }
            if (e.Key == Key.Space) // se verifică dacă este apăsată tasta Space
            {
                obiect.ResetPosition();
            }
            if (e.Key == Key.A && previousKeyboard[Key.A]) // se verifică dacă este apăsată tasta A
            {
                camera.RotesteStanga();
            }
            if (e.Key == Key.D && previousKeyboard[Key.D]) // se verifică dacă este apăsată tasta D
            {
                camera.RotesteDreapta();
            }
            if (e.Key == Key.R && !previousKeyboard[Key.R] && !e.Control) // se verifică dacă este apăsată tasta R
            {

                if(modificareCulori==ModificareCulori.FATA)
                    obiect.ModifyColorFace(true, false, false, incrementCuloare);
                else
                    obiect.ModifyColorTriangle(true, false, false,(int)modificareCulori,incrementCuloare);

            }
            if (e.Key == Key.R && !previousKeyboard[Key.R] && e.Control) // se verifică dacă este apăsată tasta R si Control
            {

                if (modificareCulori == ModificareCulori.FATA)
                    obiect.ModifyColorFace(true, false, false, -incrementCuloare);
                else
                    obiect.ModifyColorTriangle(true, false, false, (int)modificareCulori, -incrementCuloare);
            }
            if (e.Key == Key.G && !previousKeyboard[Key.G] && !e.Control) // se verifică dacă este apăsată tasta G
            {
                if (modificareCulori == ModificareCulori.FATA)
                    obiect.ModifyColorFace(false, true, false, incrementCuloare);
                else
                    obiect.ModifyColorTriangle(false, true, false, (int)modificareCulori, incrementCuloare);


            }
            if (e.Key == Key.G && !previousKeyboard[Key.G] && e.Control) // se verifică dacă este apăsată tasta G si Control
            {
                if (modificareCulori == ModificareCulori.FATA)
                    obiect.ModifyColorFace(false, true, false, -incrementCuloare);
                else
                    obiect.ModifyColorTriangle(false, true, false, (int)modificareCulori, -incrementCuloare);


            }
            if (e.Key == Key.B && !previousKeyboard[Key.B] && !e.Control) // se verifică dacă este apăsată tasta B
            {

                if (modificareCulori == ModificareCulori.FATA)
                    obiect.ModifyColorFace(false, false, true, incrementCuloare);
                else
                    obiect.ModifyColorTriangle(false, false, true, (int)modificareCulori, incrementCuloare);


            }
            if (e.Key == Key.B && !previousKeyboard[Key.B] && e.Control) // se verifică dacă este apăsată tasta B si Control
            {
                if (modificareCulori == ModificareCulori.FATA)
                    obiect.ModifyColorFace(false, false, true, -incrementCuloare);
                else
                    obiect.ModifyColorTriangle(false, false, true, (int)modificareCulori, -incrementCuloare);


            }
            if (e.Key == Key.F1 && !previousKeyboard[Key.F1]) // se verifica daca este apasata tasta F1
            {
                obiect.GenerateNewColors();
            }
            if (e.Key == Key.I && !previousKeyboard[Key.I]) // se verifica daca este apasata tasta I
            {
                Console.WriteLine(obiect.Info());
            }
            if (e.Key == Key.Number1 &&!previousKeyboard[Key.Number1]) // se verifică dacă este apăsată tasta 1
            {
                Console.WriteLine("Modificati culorile primului vertex din fiecare triunghi!");
                modificareCulori = ModificareCulori.VERTEX1;
            }
            if (e.Key == Key.Number2 && !previousKeyboard[Key.Number2]) // se verifică dacă este apăsată tasta 2
            {
                Console.WriteLine("Modificati culorile celui de al doilea vertex din fiecare triunghi!");
                modificareCulori = ModificareCulori.VERTEX2;
            }
            if (e.Key == Key.Number3 && !previousKeyboard[Key.Number3]) // se verifică dacă este apăsată tasta 3
            {
                Console.WriteLine("Modificati culorile celui de al treilea vertex din fiecare triunghi!");
                modificareCulori = ModificareCulori.VERTEX3;
            }
            if (e.Key == Key.Number0 && !previousKeyboard[Key.Number0]) // se verifică dacă este apăsată tasta 0
            {
                Console.WriteLine("Modificati culorile fetei din fata a cubului!");
                modificareCulori = ModificareCulori.FATA;
            }
            if (e.Key == Key.S && !previousKeyboard[Key.S]) // se verifica daca este apasata tasta S (salveaza in fisier) Tema 5
            {
                obiect.SaveToTxtFile();
            }
        }

        /// <summary>
        /// Setarea mediului OpenGL și a culorii fundalului ferestrei 3D
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.Enable(EnableCap.DepthTest); /// TEMA 4, folosit pentru randarea 3D
            GL.DepthFunc(DepthFunction.Less); /// TEMA 4, folosit pentru randarea 3D
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
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 512);
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
            camera.ControlCamera(mouseMovement,isMoving,left,right,up,down); 
            foreach(Obiect3D o in obiecte)
            {
                if (o != null)
                {
                    o.updatePositionFromGravity();
                }
            }
        }

        /// <summary>
        /// Secțiunea pentru randarea scenei 3D. Controlată de modulul logic din metoda ONUPDATEFRAME.
        /// Randează obiectul în funcție de stările logice ale câmpurilor <c>isMoving</c> și <c>isHidden</c>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
           
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit); /// TEMA 4, extrem de important pentru randarea 3D, la miscarea camerei unele obiecte trebuie sa fie ascunse de altele 
            axe.Draw();
            grid.Draw();
            obiect.Draw();
            for(int i = 0;i<nrCurentObiecte;i++)
                obiecte[i].Draw();
            this.SwapBuffers();
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
