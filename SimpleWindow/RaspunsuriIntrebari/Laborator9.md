# Laborator 9
## Ilculesei-Meglei Stefan grupa 3131a

Acest fișier Markdown răspunde la întrebările de la finalul laboratorului 9.
``1.Descrieți diferențele dintre iluminarea așa cum funcționează în lumea reală și modelul de iluminare utilizat de OpenGL.``
	Iluminarea utilizată de OpenGL este doar o aproximare a iluminării din realitate. Iluminarea reală este un proces complex care este greu de simulat, prin urmare sursele de lumină din OpenGL pot fi fie punctiforme, direcționale sau spot.

``2.Câte surse de lumină sunt suportate în implementarea curentă a OpenGL cu ajutorul framework-ului OpenTK?``
	În OpenGL sunt curent suportate 8 surse de iluminat.

``3.Definiți iluminarea de material și specificați unde și când este utilizată aceasta.``
Iluminarea de material se referă la modul în care un obiect reacționează la iluminarea ambientală și directă, precum și la modul în care aceasta reflectă sau absoarbe lumina. 
Acest tip de iluminare este folosit în toate domeniile de grafică pe calculator unde este dorită o iluminare realistă.

``4.Care este efectul asupra diverselor obiecte la activarea unei surse de
lumină secundare (per pct. 3), comparativ cu utilizarea unei singure
surse de lumină?``
Iluminarea de material va fi realizată dintr-o compunere a iluminării provenite din sursa principală și cea secundară.
