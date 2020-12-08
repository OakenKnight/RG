# RG
Modelovanje statičke 3D scene (prva faza): 

Uključiti testiranje dubine i sakrivanje nevidljivih površina. Definisati projekciju u perspektivi (fov=45, near=1, a vrednost far po potrebi) i viewport-om preko celog prozora unutar Resize metode.
Koristeći AssimpNet bibloteku i klasu AssimpScene, učitati model računara.Ukoliko je model podeljen u nekoliko fajlova, potrebno ih je sve učitati i iscrtati. Skalirati model, ukoliko je neophodno, tako dabude vidljiv u celosti.
Modelovati sledeće objekte: 
podlogu koristeći GL_QUADS primitivu, 
računarski sto korišćenjem Cube klase
CD korišćenjem Disk klase
Ispisati 2D tekst žutom bojom u donjem desnom uglu prozora (redefinisati viewport korišćenjem glViewport metode). Font je Tahoma, 10pt, italic i underline. Tekst treba da bude oblika: 
Predmet: Racunarska grafika 
Sk.god: 2020/21.
Ime: <ime_studenta>
Prezime: <prezime_studenta>
Sifra zad: <sifra_zadatka>
