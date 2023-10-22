# Laborator 2
## Ilculesei-Meglei Stefan grupa 3131a

Acest fisier Markdown raspunde la intrebarile de la finalul laboratorului 2.

1.Ce este un *viewport*?

Termenul viewport se refera la o regiune dreptunghiulara a ferestrei de vizualizare unde se deseneaza grafica.

2.Ce reprezinta conceptul de frames per seconds din punctul de vedere al bibliotecii OpenGL?

FPS indica numarul de cadre afisate pe secunda. 
Cu cat valoarea FPS este mai mare, cu atat interfata grafica va parea mai fluenta si mai reactiva.

3.Cand este rulata metoda OnUpdateFrame()?

Metoda onUpdateFrame ruleaza la timpul definit in parametrii metodei Run().

4.Ce este modul imediat de randare?

In modul imediat, pentru a desena obiecte grafice pe ecran, programatorul specifica direct vertex-urile si alte atribute ale acestora 
in codul aplicatiei, pentru fiecare cadru de redare.

5.Care este ultima versiune de OpenGL care accepta modul imediat?


Ultima versiune de OpenGL care suporta modul imediat de randare este OpenGL 2.1. 

6.Cand este rulata metoda OnRenderFrame()?

Metoda onUpdateFrame ruleaza la timpul definit in parametrii metodei Run().

7.De ce este nevoie ca metoda OnResize() sa fie executata cel putin
o data?

Deoarece ea va fi apelata la deschiderea aplicatiei.

8.Ce reprezinta parametrii metodei CreatePerspectiveFieldOfView() si care este domeniul de valori
pentru acestia?

Parametrii metodei sunt: 
- **fovy**  numar real (float)
- **aspect** numar real (float)
- **zNear** numar real (float)
- **zFar** numar rea (float)
- **result** un obiect de tip Matrix4



