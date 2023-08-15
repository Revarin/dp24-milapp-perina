# Beta specifikace
Tento soubor obsahuje rozvedenou (__beta__) specifikaci. Požadavky na funkce a vlastnosti aplikace jsou zde dále rozepsány. Jednotlivé požadavky jsou seřazeny do několika kategorií podle jejich důležitosti pro celkovou funkci/použití aplikace. Čím výš kategorie je, tím je důležitější. Kategorie tak naznačují jak bude zhruba probíhat implementace aplikace.

## Kategie 1. (Základní zobrazení)
- Uživatel si bude moct v aplikaci zobrazit __Google mapy__ a manipulovat s nimi (pohyb, přiblízení, oddálení).
    - _Google mapy_ budou představovat výchozí zdroj map.
- Při zapnutí GPS uživatel uvidí na mapě __svou pozici__.
    - Tato pozice se bude průběžně aktualizovat aby reflektovala pohyb uživatele.
    - Označení vlastní pozice bude v aplikaci unikátní symbol.
- V aplikaci bude na mapě vidět __poloha dalších zařízení__, na kterých je aplikace spuštěna.
    - Označená pozice bude mít jméno a symbol.
    - Jméno a symbol pro zobrazení na ostatních zařízení (tedy pro jiné) bude možné nastavit v nastavení aplikace.

## Kategorie 2. (Štáb a základní komunikace)
- V aplikaci bude možné __vytvořit akci__.
    - Zařízení, které toto provede se stane _štábním zařízením_ a bude mít práva navíc.
    - Ve štábním zařízení bude možné zobrazit __kód akce__, pomocí kterého se ostatní zařízení do akce připojí.
- Štábní zařízení bude moct __povýšit__ jiné zařízení na štábní.
    - V seznamu připojených zařízení bude možné přiřadit štábní zařízení, kterému se zodpovídá. Zařízení pak komunikuje pouze s tímto zařízením.
    - Štábní zařízení mohou komunikovat s libovolným zařízením.
- Zařízení bude moct __odeslat hlášení__ štábnímu zařízení.
- Štábní zařízení může __odeslat rozkaz__ danému zařízení.
    - Štábní zařízení může __odeslat hromadný rozkaz__ všem zařízením v terénu.
    - Hromadný rozkaz může mít různé úrovně důležitosti, podle které se na zařízení v terénu zobrazí upozornění.
- Zařízení může __odeslat zprávu__ jinému zařízení v terénu.

## Kategorie 3. (Základní značení)
- Libovolné zařízení může na mapě __vytvořit bod__.
    - Bod bude obsahovat následující vlatnosti: název, datum vytvoření, (typ?), popis, přiložené soubory, komentáře a grafickou značku.
    - Vytvořený bod může editovat nebo smazat pouze autor nebo štábní zařízení.
    - Ostatní zařízení mohou k objektu psát komentáře.
- V aplikaci budou __předdefinované grafické značení__ pro standardní druhy bodů.
- V aplikaci bude editor pro __vytváření nových grafických značení__ (z jednotlivých prvků)
    - Značení bude složeno z následujících částí: tvar, barva, značka, (ozdoba?).
    - Při vytváření bodu bude možné vybrat pouze z předdefinovaných nebo vytvořených značení.
    - Každé zařízení může vytvářet vlastní značení (práva?).
    - Štábní zařízení může sdílet své vytvořené značení ostatním zařízením.

## Kategorie 4. (Zdroje map)
- V nastavení zařízení bude možné vybrat z __více zdrojů online map__.
- Do zařízení bude možné importovat vlastní mapy. Import bude dvojího druhu:
    - Globální import uloží mapy na server pod kódem akce. Ty se tak stanou dostupné pro všechny zařízení, které jsou součástí dané akce.
    - Lokální import ponechá mapu pouze na zařízení, pouze se začne používat v aplikaci.

## Kategorie 5. (Pokročilá komunikace)
- Při komunikace bude možné využívat __šablony zpráv__, podle kterých je zpráva sestavena.
    - Šablony bude složena z pevných textových částí a z polích pro doplnění textu různého formátu. Zpráva tak bude sestavena formou formuláře.
    - Při psaní zprávy bude možné vybrat z předdefinovaných nebo vytvořených šablon hlášení.
- V aplikaci budou __předdefinované šablony zpráv__ pro standardní druhy hlášení.
- V aplikaci bude editor pro __vytváření nových šablon zpráv__.
    - Šablona bude skládána z posloupnosti pevného textu a volných polí s různými omezeními - pouze jednoduché šablony.
    - Každé zařízení může vytvářet vlastní šablony hlášení (práva?).
    - Štábní zařízení může sdílet své vytvořené šablony ostatním zařízením.
- Při komunikaci bude možné pomocí speciálního tlačítka sdílet __souřadnice vlastní polohy__.
- Na mapě bude možné __kopírovat souřadnice__ vybrané lokace a ty pak __vložit__ do zprávy v daném formátu.

## Kategorie 6. (Pokročilé značení)
- Libovolné zařízení může na mapě __vytvořit plochu__.
    - Plocha bude obsahovat následující vlastnosti: název, datum vytvoření, (typ?), popis, přiložené soubory, komentáře a grafickou značku.
    - Vytvořenou plochu může editovat nebo smazat pouze autor nebo štábní zařízení.
    - Ostatní zařízení mohou k objektu psát komentáře.
- Libovolné zařízení může na mapě __vytvořit cestu__.
    - Cesta bude obsahovat následující vlastnosti: název, datum vytvoření, (typ?), popis, přiložené soubory, komentáře a grafickou značku.
    - Vytvořenou cestu může editovat nebo smazat pouze autor nebo štábní zařízení.
    - Ostatní zařízení mohou k objektu psát komentáře.
- V aplikace budou __předdefinované grafické značení__ pro standardní druhy ploch a cest.
    - V aplikaci nebude možné vytvářet nové grafické značení pro plochy a cesty.

## Kategorie 7. (Skupiny)
- Zařízení může být __přiřazené k 0-N skupinám__.
    - Skupina je označena názvem, barvou, značkou a má X členů.
    - Členové v jedné skupině jsou na mapě speciálně označeni značkou nebo barvou skupiny.
    - Zařízení ve více skupinách je na mapě označeno svou hlavní skupinou.
- Zařízení ve skupině mohou přímo komunikovat ve __skupinovém chatu__.
    - Do skupinového chatu mají přístup pouze členové dané skupiny a štábní zařízení.
- Štábní zařízení mohou __spravovat skupiny__: vytvářit novou skupinu, přidat zařízení do skupiny, odebrat zařízení ze skupiny a rozpustit skupiny.
    - Štábní zařízení mohou nastavit vzájemnou viditelnost pozice, mapových značek mezi skupinami.

## Kategorie 8. (Další)
- Štábní zařízení bude moct na dálku přerušit přístup vybraného zařízení k veškerým datům akce - zařízení je z akce vyloučeno.
- Při vytvoření značení bude možné zadat časovač, po jehož uplynutí je značení automaticky odstraněno z mapy.
- Při vytvoření značení bude možné automaticky zaslat notifikace všem zařízení nebo zařízením dané skupině.
- Možnost v aplikace nastavit číselnou šifru.
    - Tuto šifru pak bude možné použít v chatu pro rychlé zašifrování vybrané části zprávy.
- V chatu bude možné posílat soubory.
    - Obrázky bude možné v chatu přímo zobrazit.
    - Videa a zvuk bude možné v chatu přímo přehrát.
    - Ostatní soubory bude možné uložit na zařízení.
- Možnost exportu aktuálního stavu mapy (vytvořené body, plochy, cesty a poloha všech zařízení).
    - Formát GPX.
