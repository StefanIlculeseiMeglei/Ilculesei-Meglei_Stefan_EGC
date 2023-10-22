# Laborator 2
## Ilculesei-Meglei Ștefan grupa 3131a

Acest fișier Markdown răspunde la întrebările de la finalul laboratorului 2.

1.Ce este un *viewport*?

Termenul viewport se referă la o regiune dreptunghiulară a ferestrei de vizualizare unde se desenează grafica.

2.Ce reprezintă conceptul de frames per seconds din punctul de vedere al bibliotecii OpenGL?

FPS indică numărul de cadre afișate pe secundă. 
Cu cât valoarea FPS este mai mare, cu atât interfața grafică va părea mai fluentă și mai reactivă.

3.Când este rulată metoda OnUpdateFrame()?

Metoda onUpdateFrame rulează la timpul definit în parametrii metodei Run().

4.Ce este modul imediat de randare?

În modul imediat, pentru a desena obiecte grafice pe ecran, programatorul specifica direct vertex-urile și alte atribute ale acestora 
în codul aplicației, pentru fiecare cadru de redare.

5.Care este ultima versiune de OpenGL care acceptă modul imediat?


Ultima versiune de OpenGL care suportă modul imediat de randare este OpenGL 2.1. 

6.Când este rulată metoda OnRenderFrame()?

Metoda onUpdateFrame rulează la timpul definit în parametrii metodei Run().

7.De ce este nevoie ca metoda OnResize() să fie executată cel puțin
o dată?

Deoarece ea va fi apelată la deschiderea aplicației.

8.Ce reprezintă parametrii metodei CreatePerspectiveFieldOfView() și care este domeniul de valori
pentru aceștia?

Parametrii metodei sunt: 
- **fovy**  număr real (float)
- **aspect** număr real (float)
- **zNear** număr real (float)
- **zFar** număr rea (float)
- **result** un obiect de tip Matrix4



