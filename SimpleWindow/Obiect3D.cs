using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWindow
{
    /// <summary>
    /// Clasa tratează obiectele care trebuie desenate. Face parte din Tema 3, Tema 4 și Tema 5
    /// </summary>
    internal class Obiect3D
    {
       /// <summary>
       /// Câmpul <c>caleFisier</c> reprezintă calea către fișierul sursă al obiectului, în acest caz este un fișier text
       /// </summary>
        string caleFisier = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, "assets/cub.txt");
        /// <summary>
        /// Câmpul <c>FACTOR_SCALARE</c> reprezintă cu cât vom scala obiectul desenat
        /// </summary>
        private const int FACTOR_SCALARE = 10;
        /// <summary>
        /// Câmpul este folosit pentru a delimita zona în care obiectele pot fi generate, face parte din Tema 5 (subpunct 1)
        /// </summary>
        private const int DISTANTA_RAND_MAX = 70;
        /// <summary>
        /// Câmpul <c>coordsList</c> este o listă care conține vertexurile ce definesc obiectul desenat.
        /// </summary>
        private List<Vector3> coordsList;
        /// <summary>
        /// Câmpul <c>originalCoords</c> este o copie a vertexurilor inițiale ale obiectului
        /// </summary>
        private List<Vector3> originalCoords;
        /// <summary>
        /// Boolean care decide dacă obiectul este desenat sau nu
        /// </summary>
        private bool visibility;
        /// <summary>
        /// Boolean care constată dacă a apărut o eroare sau nu
        /// </summary>
        private bool hasError;
        /// <summary>
        /// Câmpul <c>culori</c> este un array care conține culorile fiecărui vertex ce definește obiectul
        /// </summary>
        Color[] culori;
        /// <summary>
        /// Câmp care determină daca obiectul a atins planul XOY. Folosit în tema 5
        /// </summary>
        float aboluteLowerY;
        /// <summary>
        /// Proprietate care determină dacă obiectul este afectat de gravitate
        /// </summary>
        public bool gravity { get; set; }
        /// <summary>
        /// Constructor 
        /// </summary>
        private const float fallingSpeed = 1f;
        public Obiect3D()
        {
            try
            {
                gravity = false;
                aboluteLowerY = int.MaxValue;
                coordsList = LoadFromTxtFile(caleFisier);
                MoveAboveXOY();
                originalCoords = new List<Vector3>();
                InitCulori();
                for(int i = 0; i < coordsList.Count; i++)
                {
                    originalCoords.Add(coordsList[i]);
                }
                if (coordsList.Count == 0)
                {
                    Console.WriteLine("Crearea obiectului a esuat: obiect negasit/coordonate lipsa!");
                    return;
                }
                visibility = false;
                hasError = false;
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: assets file <" + caleFisier + "> is missing!!!");
                hasError = true;
            }
        }
        /// <summary>
        /// Metoda care modifică poziția obiectului în cazul în care este afectat de gravitate. Face parte din Tema 5 (subpunct 1)
        /// </summary>
        public void updatePositionFromGravity()
        {
            if(gravity)
            {
                Translate(0,-fallingSpeed, 0);
            }

        }
        /// <summary>
        /// Aceasta metoda initializarea culorile pentru fiecare vertex, deoarece 6 vertexuri consecutive definesc o fata a cubului ele vor avea aceasi culoare
        /// TEMA 4. Subpuctul 3
        /// </summary>
        /// <param name="dim"></param>
        private void InitCulori() 
        {
            culori = new Color[coordsList.Count];
            Random rand = new Random();
            Color cul = new Color();
            for (int i = 0; i < culori.Length; i++)
            {
               if(i%6==0) // fetele cubului vor avea aceeasi culoare, conditia functioneaza deoarece o fata este formata din 6 vertexuri consecutive
                  cul= Color.FromArgb( rand.Next(255), rand.Next(255), rand.Next(255));
                culori[i]= cul;
            }
        }
        /// <summary>
        /// Metoda care aduce obiectul în poziția inițială
        /// </summary>
        public void ResetPosition()
        {
            for(int i = 0;i < coordsList.Count;i++)
            {
                coordsList[i] = originalCoords[i]; 
            }
        }

        public void ToggleVisibility()
        {
            if (hasError == false)
            {
                visibility = !visibility;
            }
        }
        /// <summary>
        ///  Metodă care translatează obiectul pe o poziție random aleasă într-un anumit interval, face parte din Tema 5 (subpunct 1)
        ///  Folosit pentru a genera obiecte noi pe poziții aleatorii
        /// </summary>
        public void TranslateRandom()
        {
            Random random = new Random();
            int newX= random.Next(2) > 0 ? random.Next(DISTANTA_RAND_MAX) : -random.Next(DISTANTA_RAND_MAX);
            while (Math.Abs(coordsList[0].X-newX)<FACTOR_SCALARE*2) // obiectul sa nu se genereze peste cel initial
                newX = random.Next(2) > 0 ? random.Next(DISTANTA_RAND_MAX) : -random.Next(DISTANTA_RAND_MAX);
            int newZ = random.Next(2) > 0 ? random.Next(DISTANTA_RAND_MAX) : -random.Next(DISTANTA_RAND_MAX);
            while (Math.Abs(coordsList[0].Z - newZ) < FACTOR_SCALARE*2) // obiectul sa nu se genereze peste cel initial
                newZ = random.Next(2) > 0 ? random.Next(DISTANTA_RAND_MAX) : -random.Next(DISTANTA_RAND_MAX);
            int newY = random.Next(DISTANTA_RAND_MAX);
            while (newY < FACTOR_SCALARE)
                newY = random.Next(DISTANTA_RAND_MAX);
            Translate(newX,newY, newZ);

        }
        /// <summary>
        /// Metodă care modifică culoare unui fețe a obiectului desenat, face parte din Tema 4(Subpunct 1)
        /// </summary>
        /// <param name="red">canalul rosu este afectat</param>
        /// <param name="green">canalul verde este afectat</param>
        /// <param name="blue">canalul albastru este afectat</param>
        /// <param name="value">valoarea cu care este afectata</param>
        /// <param name="apha">canalul alpha este afectat</param>

        public void ModifyColorFace(bool red, bool green, bool blue, int value, bool apha = false)
        {
            int rez = 0;
            for(int i=0;i<6;i++) // fata este definita de primele 6 vertexuri
            {
                if(red)
                {
                    rez = culori[i].R + value;
                    rez = rez <0?0:rez>255?255:rez; // limitele pentru culori
                    culori[i] = Color.FromArgb(culori[i].A,rez, culori[i].G, culori[i].B);
                }
                if(green)
                {
                    rez = culori[i].G + value;
                    rez = rez < 0 ? 0 : rez > 255 ? 255 : rez; // limitele pentru culori
                    culori[i] = Color.FromArgb(culori[i].A, culori[i].R, rez, culori[i].B);
                }
                if (blue)
                {
                    rez = culori[i].B + value;
                    rez = rez < 0 ? 0 : rez > 255 ? 255 : rez; // limitele pentru culori
                    culori[i] = Color.FromArgb(culori[i].A, culori[i].R, culori[i].G, rez);
                }
                if (apha)
                {
                    rez = culori[i].A + value;
                    rez = rez < 0 ? 0 : rez > 255 ? 255 : rez; // limitele pentru culori
                    culori[i] = Color.FromArgb(rez, culori[i].R, culori[i].G, culori[i].B);
                }
            }
        }
        /// <summary>
        /// Metocă care modifică culoare unui vertex din fiecare triunghi, face parte din Tema 3(subpunct 9) și Tema 4(subpunct 2)
        /// </summary>
        /// <param name="red">canalul rosu este afectat</param>
        /// <param name="green">canalul verde este afectat</param>
        /// <param name="blue">canalul albastru este afectat</param>
        /// <param name="vertex">ce vertex va fi afectat, valori între 1 si 3</param>
        /// <param name="value">valoarea cu care este afectata</param>
        /// <param name="apha">canalul alpha este afectat</param>
        /// <exception cref="ArgumentException">se accept doar valori între 1 și 3 pentru vertex</exception>
        public void ModifyColorTriangle(bool red,bool green,bool blue,int vertex,int value,bool apha=false)
        {
            if (vertex < 1 || vertex > 3)
                throw new ArgumentException("Vertex incorect, trebuie sa fie 1,2 sau 3");
            int rez = 0;
            for (int i = vertex-1; i < coordsList.Count; i=i+3) // fiecare triunghi este format din 3 vertexuri consecutive
            {
                if (red)
                {
                    rez = culori[i].R + value;
                    rez = rez < 0 ? 0 : rez > 255 ? 255 : rez; // limitele pentru culori
                    culori[i] = Color.FromArgb(culori[i].A, rez, culori[i].G, culori[i].B);
                }
                if (green)
                {
                    rez = culori[i].G + value;
                    rez = rez < 0 ? 0 : rez > 255 ? 255 : rez; // limitele pentru culori
                    culori[i] = Color.FromArgb(culori[i].A, culori[i].R, rez, culori[i].B);
                }
                if (blue)
                {
                    rez = culori[i].B + value;
                    rez = rez < 0 ? 0 : rez > 255 ? 255 : rez; // limitele pentru culori
                    culori[i] = Color.FromArgb(culori[i].A, culori[i].R, culori[i].G, rez);
                }
                if (apha)
                {
                    rez = culori[i].A + value;
                    rez = rez < 0 ? 0 : rez > 255 ? 255 : rez; // limitele pentru culori
                    culori[i] = Color.FromArgb(rez, culori[i].R, culori[i].G, culori[i].B);
                }
            }


        }
        /// <summary>
        /// Metoda generează noi culori pentru fețele obiectului. Face parte din Tema 4 subpunctul 3
        /// </summary>
        public void GenerateNewColors()
        {
            MoveAboveXOY();
            InitCulori();
        }
        /// <summary>
        /// Metoda care desenează obiectul
        /// </summary>

        public void Draw()
        {
            if (hasError == false && visibility == true)
            {
                GL.Begin(PrimitiveType.Triangles);
                for(int i = 0; i < coordsList.Count;i++) {
                    GL.Color3(culori[i]);
                    GL.Vertex3(coordsList[i]);
                }
                GL.End();
            }
        }
        /// <summary>
        /// Metoda ca translateaza obiectul, face parte din tema 2
        /// </summary>
        /// <param name="x"> translatare pe axa X</param>
        /// <param name="y"> transalatare pe axa Y</param>
        /// <param name="z"> transalatare pe axa Z</param>

        public void Translate(float x, float y, float z)
        {
            if (aboluteLowerY + y >= 0)
            {
                aboluteLowerY += y;
                for (int i = 0; i < coordsList.Count; i++)
                {
                    coordsList[i] = new Vector3(x + coordsList[i].X, y + coordsList[i].Y, z + coordsList[i].Z);
                }
            }
            else
            {
                for (int i = 0; i < coordsList.Count; i++)
                {
                    coordsList[i] = new Vector3(x + coordsList[i].X, coordsList[i].Y, z + coordsList[i].Z);
                }

            }
        }
        public String Info()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Informatii despre obiect:");
            sb.AppendLine("Obiectul este format din " + coordsList.Count + " vertexuri.");
            sb.AppendLine("Valorile RGB a vertexurilor:");
       
            for (int i = 0; i < coordsList.Count; i++)
            {
             sb.AppendLine("Vertex[" + i + "] are valorile RGB:{" + culori[i].R + "," + culori[i].G + "," + culori[i].B + "}");

           }
           return sb.ToString();
        }
        /// <summary>
        /// Metodă de inițializare a coordonatelor pentru un obiect, coordonatele sunt citite dintr-un fișier. Face parte din Tema 3(subpunct 8) și Tema 4(subpunct 1)
        /// </summary>
        /// <param name="fname"> numele fisierului</param>
        /// <returns> o lista cu toate vertexurile </returns>
        private List<Vector3> LoadFromTxtFile(string fname) 
        {
            List<Vector3> vlc3 = new List<Vector3>();


            var lines = File.ReadLines(fname);
            foreach (var line in lines)
            {
                if (line.Trim().Length > 2)
                {
                    string ch1 = line.Trim().Substring(0, 1);
                    string ch2 = line.Trim().Substring(1, 1);
                    if (ch1 == "v" && ch2 == " ")
                    {

                        string[] block = line.Trim().Split(' ');
                        if (block.Length == 4)
                        {
                            // ATENTIE: Pericol!!!
                            float xval = float.Parse(block[1].Trim()) * FACTOR_SCALARE;
                            float yval = float.Parse(block[2].Trim()) * FACTOR_SCALARE;
                            float zval = float.Parse(block[3].Trim()) * FACTOR_SCALARE;
                            aboluteLowerY = Math.Min(aboluteLowerY,(int)yval); // face parte din tema 5

                            vlc3.Add(new Vector3((int)xval, (int)yval, (int)zval));
                        }
                    }
                }
            }

            return vlc3;
        }
        /// <summary>
        /// Metoda care salveaza coordonatele intr-un fisier, face parte din Tema 5 (subpunct 3)
        /// </summary>
        public void SaveToTxtFile()
        {
            try
            {

                StreamWriter f = File.CreateText(caleFisier);
                foreach (Vector3 ver in coordsList)
                {
                    f.WriteLine("v " + ver.X / FACTOR_SCALARE + " " + ver.Y / FACTOR_SCALARE + " " + ver.Z / FACTOR_SCALARE);
                }
                f.Close();
            }
            catch (Exception) // Testare de erori la scriere
            {
                Console.WriteLine("ERROR: assets file <" + caleFisier + "> cannot be opened!!");
                hasError = true;
            }
        }
        /// <summary>
        /// Metoda muta obiectul deasupra planului XOY, folosit în tema 5
        /// </summary>
        private void MoveAboveXOY()
        {
            if(aboluteLowerY<0)
            {
                for (int i = 0; i < coordsList.Count; i++)
                {
                    coordsList[i] = new Vector3(coordsList[i].X, coordsList[i].Y +Math.Abs(aboluteLowerY), coordsList[i].Z);
                }
                aboluteLowerY = 0;

            }
        }

    }
}
