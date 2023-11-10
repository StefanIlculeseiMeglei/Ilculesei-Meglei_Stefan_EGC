# Laborator 3
## Ilculesei-Meglei Stefan grupa 3131a

Acest fișier Markdown răspunde la întrebările de la finalul laboratorului 3.

``1. Care este ordinea de desenare a vertexurilor pentru aceste metode (orar sau anti-orar)?``

 Ordinea de desenare anti-orar este cea default pentru openGL.

``2. Ce este anti-aliasing? Prezentați această tehnică pe scurt.``
  
  Anti-aliasing este o tehnică utilizată în grafica pe calculator pentru a reduce efectul de zimțare a liniilor,
  ea funcționează prin interpolarea culorilor sau nivelurilor de intensitate între pixelii învecinați pentru a crea o tranziție mai lină.

``3. Care este efectul rulării comenzii GL.LineWidth(float)? Dar pentru GL.PointSize(float)? Funcționează în interiorul unei zone GL.Begin()?``

  GL.LineWidth(float) modifică grosimea liniilor care vor fi desenate, iar GL.PointSize(float) modifică mărimea punctelor care vor urma să fie desenate. Aceste comenzi nu funcționează într-un block GL.Begin() GL.End().

``4. Răspundeți la următoarele întrebări: ``
- Care este efectul utilizării directivei LineLoop atunci când
sunt desenate segmente de dreaptă multiple în OpenGL?

	Directiva LineLoop desenează un contur închis, unde liniile sunt conectate consecutiv, iar ultima este conectată la prima.
- Care este efectul utilizării directivei LineStrip atunci când
sunt desenate segmente de dreaptă multiple în OpenGL?
	
	Directiva LineStrip funcționează similar cu LineLoop, diferența dintre cele două constă în faptul că LineStrip nu mai conectează prima linie cu ultima.
- Care este efectul utilizării directivei TriangleFan atunci când
sunt desenate segmente de dreaptă multiple în OpenGL?
	Directiva TriangleFan desenează triunghiuri cu un vârf comun, primul punct desenat, celelate drepte vor forma un triunghi un primul punct desenat. În final forma va fi un evantai.
- Care este efectul utilizării directivei TriangleStrip atunci când
sunt desenate segmente de dreaptă multiple în OpenGL?
	Directiva TriangleStrip desenează câte un triunghi pentru fiecare trei puncte consecutive.

``6. De ce este importantă utilizarea de culori diferite (în gradient sau
culori selectate per suprafață) în desenarea obiectelor 3D?``

Este important să folosim culori diferite pentru a evidenția conturile și suprafața obiectelor. Gradientul de culori pote fi folosit și pentru a da iluzia de iluminat.

``7. Ce reprezintă un gradient de culoare? Cum se obține acesta în
OpenGL?``

Un gradient de culoare reprezintă o tranziție treptată între două sau mai multe culori. În OpenGL, un gradient de culoare poate fi obținut prin intermediul shaderelor.

``9. Ce efect are utilizarea unei culori diferite pentru fiecare vertex
atunci când desenați o linie sau un triunghi în modul strip?``

Atunci când folosim culori diferite pentru fiecare vertex, linia sau triunghiul desenat va fi unul gradient fiecare pixel fiind rezultat prin interpolarea culorilor vertexurilor.

