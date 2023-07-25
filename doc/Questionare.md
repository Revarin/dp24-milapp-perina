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
Toto jsou prvky, které mohou a nemusí být implementovány. Poprosil bych ohodnotit 1-5 (1 - důležité, 2 - užitečné, 3 - neutrální, 4 - nepodstatné, 5 - zbytečné/škodlivé)

- Podpora zařízení pro více členů týmu. Vybraní členi budou 'masters' - budou mít normální přístup k aplikaci. Ostatní budou 'slaves' - jejich přístup bude omezený. Poloha 'slaves' bude zobrazena pouze jejich 'masters'. Nastavení práv 'slaves' bude v nastavení aplikaci.
- Skupiny. Zařízení může být přiřazeno do větší skupiny. Skupina má jméno, značení, barvu, popis... Možnost nastavit vzájemnou viditelnost skupin na mapě, možnost posílání vzájemných zpráv, viditelnost značení vytvořené různými skupinami atd.
- Vzdálené zrušení přístupu vybraného zařízení. Štáb vzdáleně zruší přístup nějakého zařízení. Zařízení neuvidí polohu ostatních, vytvořený značení/plochy, záznamy zpráv (vhodné pokud se zařízení ztratí/padne do rukou nepřítele).
- Skupinová komunikace. Soukromý chatroom pro všechny zařízení ve skupině. Štáb má přístup.
- Privátní komunikace. Soukromá komunikace mezi dvěmi zařízeními. Štáb má přístup.
- Časovač značení, po jehož uplynutí se značení automaticky odstraní.
- Podpora šifer. V nastavení aplikace se zadá šifra (jen pro čísla/pro všechno). V chatu bude poté možné označit text a aplikovat na něj šifru. Možná podpora i více různých šifer.
- Nástup/výstup do vozidla. Značkaa týmu se změní a aplikace bude zvýší frekvenci snímání pozice.

## Návr grafického rozhraní
TODO (asi příště)
