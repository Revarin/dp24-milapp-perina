# Beta specifikace
Tento soubor obsahuje rozvedenou (__beta__) specifikaci. Požadavky na funkce a vlastnosti aplikace jsou zde dále rozvedeny. Jednotlivé požadavky jsou seřazeny do několika kategorií, které důležitost pro celkovou funkci/použití aplikace. Čím výš kategorie je, tím je důležitější. Kategorie tak naznačují jak bude zhruba probíhat implementace aplikace.

## Kategie 1. (Základní zobrazení)
- Uživatel si bude moct v aplikaci zobrazit __Google mapy__ a manipulovat s nimi (pohyb, přiblízení, oddálení).
    - _Google mapy_ budou představovat výchozí zdroj map.
- Při zapnutí GPS uživatel uvidí na mapě __svou pozici__.
    - Tato pozice se bude průběžně aktualizovat aby reflektovala pohyb uživatele.
    - Označení vlastní pozice bude v aplikaci unikátní symbol.
- V aplikaci bude na mapě vidět __poloha dalších zařízení__, na kterých je aplikace spuštěna.
    - Označená pozice bude mít jméno a symbol.
    - Jméno a symbol pro ostatní bude možné nastavit v nastavení aplikace.

## Kategorie 2. (Štáb a zprávy)
- V aplikaci bude možné __vytvořit akci__.
    - Zařízení, které toto provede se stane _štábním zařízením_ a bude mít práva navíc.
    - Ve štábním zařízení bude možné zobrazit __kód akce__, pomocí kterého se ostatní zařízení do akce připojí.
- Štábní zařízení bude moct __povýšit__ jiné zařízení na štábní.
    - V seznamu připojených zařízení bude možné přiřadit štábní zařízení, kterému se zodpovídá. Zařízení pak komunikuje pouze s tímto zařízením.
    - Štábní zařízení mohou komunikovat s libovolným zařízením.
- Zařízení bude moct __odeslat hlášení__ štábnímu zařízení.
- Štábní zařízení může __odeslat rozkaz__ danému zařízení.
    - Štábní zařízení může __odeslat hromadný rozkaz__ všem zařízením v terénu.
- Zařízení může __odeslat zprávu__ jinému zařízení v terénu.

## Kategorie 3. (Body na mapě)
- Libovolné zařízení může na mapě __vytvořit bod__.
    - Bod bude obsahovat následující vlatnosti: název, datum vytvoření, (typ?), popis, přiložené soubory, komentáře a grafickou značku.
    - Vytvořený bod může editovat nebo smazat pouze autor nebo štábní zařízení.
    - Ostatní zařízení mohou k objektu psát komentáře.
- V aplikaci budou __předdefinované grafické značení__ pro různé druhy bodů
- V aplikaci bude editor pro __vytváření nových grafických značení__ (z jednotlivých prvků)
    - Značení bude složeno z následujících částí: tvar, barva, značka, (ozdoba?).
    - Při vytváření bodu bude možné vybrat pouze z předdefinovaných nebo vytvořených značení.
    - Každé zařízení může vytvářet vlastní značení (práva?).
    - Štábní zařízení může sdílet své vytvořené značení ostatním zařízením.
