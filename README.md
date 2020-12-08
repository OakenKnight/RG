# Projektni zadatak 6.2–Ubacivanje CD-a u računar
## Modelovanje statičke 3D scene (prva faza): 

* Uključiti testiranje dubine i sakrivanje nevidljivih površina. Definisati projekciju u perspektivi (fov=45, near=1, a vrednost far po potrebi) i viewport-om preko celog prozora unutar Resize metode.

* Koristeći AssimpNet bibloteku i klasu AssimpScene, učitati model računara.Ukoliko je model podeljen u nekoliko fajlova, potrebno ih je sve učitati i iscrtati. Skalirati model, ukoliko je neophodno, tako dabude vidljiv u celosti.

* Modelovati sledeće objekte: 
1. podlogu koristeći GL_QUADS primitivu, 
2. računarski sto korišćenjem Cube klase
3. CD korišćenjem Disk klase

* Ispisati 2D tekst žutom bojom u donjem desnom uglu prozora (redefinisati viewport korišćenjem glViewport metode). Font je Tahoma, 10pt, italic i underline. Tekst treba da bude oblika: 
1. Predmet: Racunarska grafika 
2. Sk.god: 2020/21.
3. Ime: <ime_studenta>
4. Prezime: <prezime_studenta>
5. Sifra zad: <sifra_zadatka>
