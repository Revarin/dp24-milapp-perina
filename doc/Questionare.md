# Otázky a návrhy
## Cíl aplikace
Aplikace bude přinejmenším umožňovat následující případy užití:
- Sledování vlastní pozice na mapě
- Zobrazní pozic ostatních zařízení s aplikací na mapě
- Vytváření bodů a ploch na mapě s názvem, popisem, fotkami
- Komunikace se štábem (hlášení a rozkazy)
- Komunikace s ostatními tými

Aplikace je zaměřena pro zájmové skupiny. Bude určena pro akce/operace do +-100 lidí. Bude určena pouze pro pěchotu (žádná podpora pro dělostřelectvo, námořnictvo, letectvo a bojový vozidla mimo transport). Aplikace bude sloužit pouze pro plánování, monitorování a komunikaci - nebude podporovat aktivní senzory.

## Prvky
### Povinné
Tyto prvky budou implementovány:
- Zobrazení pozice zařízení na mapě (GPS)
- Zobrazení pozic ostatních zařízení na mapě (GPS)
- Vytváření bodů na mapě s názvem, popisem, přiloženými soubory (obrázky)
- Vytváření ploch na mapě s názvem, popisem, přiloženými soubory (obrázky)
- Soukromá komunikace se štábem
- Globální komunikace od štábu (upozornění)
- Využití veřejných online map
- Načtení vlastních map
- Šablony zpráv a editor šablon zpráv (šablony nebudou textové, ale jako formulář)
- Šablony značek a editor značek
- Různé formáty geo souřadnic a času

### Volitelné
Toto jsou prvky, které mohou a nemusí být implementovány. Ohodnotit 1-5 (1 - důležité, 2 - užitečné, 3 - neutrální, 4 - nepodstatné, 5 - zbytečné/škodlivé).

- Podpora zařízení pro více členů týmu. Vybraní členi budou 'masters' - budou mít normální přístup k aplikaci. Ostatní budou 'slaves' - jejich přístup bude omezený. Poloha 'slaves' bude zobrazena pouze jejich 'masters'. Nastavení práv 'slaves' bude v nastavení aplikaci.
- Skupiny. Zařízení může být přiřazeno do větší skupiny. Skupina má jméno, značení, barvu, popis... Možnost nastavit vzájemnou viditelnost skupin na mapě, možnost posílání vzájemných zpráv, viditelnost značení vytvořené různými skupinami atd.
- Vzdálené zrušení přístupu vybraného zařízení. Štáb vzdáleně zruší přístup nějakého zařízení. Zařízení neuvidí polohu ostatních, vytvořený značení/plochy, záznamy zpráv (vhodné pokud se zařízení ztratí/padne do rukou nepřítele).
- Skupinová komunikace. Soukromý chatroom pro všechny zařízení ve skupině. Štáb má přístup.
- Privátní komunikace. Soukromá komunikace mezi dvěmi zařízeními. Štáb má přístup.
- Časovač značení, po jehož uplynutí se značení automaticky odstraní.
- Podpora šifer. V nastavení aplikace se zadá šifra (jen pro čísla/pro všechno). V chatu bude poté možné označit text a aplikovat na něj šifru. Možná podpora i více různých šifer.
- Nástup/výstup do vozidla. Značka týmu se změní a aplikace bude zvýší frekvenci snímání pozice.
- Možnost vyhledat lokalitu podle adresy (způsobem jak google mapy)

## Práce s aplikací
Tato část obsahuje hrubou představu, jak by se mohl s aplikací pracovat. Velmi provizorní. Zjistit názory zda ok/neok, případně jak by se s tím pracovalo lépe.

- Centrální obrazovka bude obsahovat mapu se symboly. S mapou se bude pracovat standardním způsobem, jak v ostatních mapách. Při spuštění se mapa vycentruje na aktuální pozici (nebo poslední známou pozici).
- Zadávání bodů vybrat jeden ze dvou způsobů:
    - Podržet místo na mapě pro zobrazení nabídky (bod, plocha, cesta?). Následně se zobrazí formulář vytvoření/editace bodu/plochy.
    - Dát střed obrazovky (zaměřovač) na danou pozici použít tlačítko vytvoři bod/plochu/cestu (buď více tláčítek nebo dropdown menu). Následně se zobrazí formulář vytvoření/editace.
- Komunikace na vlastní stránce _kontakty_. Bude obsahovat všechny viditelné zařízení a vždy štáb. Po zvolení adresáta se zobrazí chatroom. Bude také možné zobrazit chatroom podržením symbolu daného zařízení na mapě a zvolení _napsat zprávu_.
    - Štáb bude mít navíc globální kontakt, který pošle zprávu všem.
- Chatroom bude standardního stylu (ála messenger nebo podobné). Text by mohlo být možné lehce formátovat (tučný text, kurzíva, podtržení, přeškrtnutí) - nejspíš stylem markdown.
- V chatroomu bude tlačítko _report_, které otevře formulář hlášení. Zde si uživatel vybere jaké hlášení chce poslat a vyplní detaily. Vše ve formě formuláře (textové pole, číselné pole, volba z N možností...)
- V chatroomu bude tlačítko _cipher_, která se povolí při označení textu. Při jeho kliknutí se dané text zašifruje pomocí zvolené šifry.

## Návr grafického rozhraní
TODO (asi příště)
