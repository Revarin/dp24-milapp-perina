## 1. Jednoduchá Google mapa
- Mapa podporuje základní funkcionalitu: pohyb po mapě, přibližování a oddalování.
- Migrace na .NET MAUI
- Aplikace si pamatuje pozici na mapě (po překliknutí na jinou záložku, po vypnutí a spuštění aplikace).

## 2. Základní GPS
- Aplikace požádá o povolení ke GPS.
- Na mapě je zobrazena pozice zařízení výchozím symbolem Google map.
- Při pohybu se zařízením je tato pozice průběžně aktualizována.
- V nastavení aplikace lze nastavit interval aktualizace GPS pozice. Toto nastavení je persistentně uloženo
- Na mapě je tlačítko pro zobrazení vlastní pozice.

## 3. Multiplayer
- V nastavení aplikace lze nastavit jméno zařízení.
- Na serveru je uložen seznam uživatelů s jejich jmény.
- Na server se periodicky posílá aktuální poloha zařízení
- Ze serveru se periodicky stahují polohy ostatních zařízení. Stahují se pouze data, která se od posledního stažení změnila.
- Na mapě budou speciálním symbolem označeny polohy všech ostatních zařízení.

## 4. Stažené mapy
- Server ukládá mapy (prozatím jednu).
- Server poskytuje rozhraní pro získání dlaždice mapy podle koordinát.
- V nastavení aplikace lze vybrat zdroj map (prozatím: Google Street, Google Satellite, Custom).
- Při výběru Custom se ze serveru budou stahovat dlaždice, které se aplikují jako překryv na Google mapě.