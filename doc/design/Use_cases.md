# Případy použití aplikace
Tento dokumentu obsahuje rozepsané případy užití aplikace RAT. Případy použití budou zapsány ve strukturovaném formátu popsaný níže:

| Název | Popis |
| - | - |
| __JMÉNO__ | Název případu užití
| __DŮLEŽITOST__ | Jak moc případ užití důležitý (1 (min) - 5 (max))
| __AKTÉR__ | Kdo provádí danou operaci
| __PRECONDITIONS__ | Předpoklady platné před spuštěním operace
| __POSTCONDITIONS__ | Předpoklady platné po dokončení operace
| __SCÉNÁŘ__ | Hlavní a alternativní scénáře

## Zjištění aktuální polohy
|  |  |
| - | - |
| __JMÉNO__ | Zjištění polohy
| __DŮLEŽITOST__ | 5
| __AKTÉR__ | Voják v terénu
| __PRECONDITIONS__ | Zařízení s aplikací je vybavené GPS anténou, která je zapnutá a aplikace má k ní povolený přístup.
| __POSTCONDITIONS__ | Žádné
| __SCÉNÁŘ__ | Uživatel zapne aplikaci. Po jejím načtení se zobrazí mapa, která se vystředí na aktuální polohu zařízení.
| __ALT SCÉNÁŘ 1__ | Aplikace požádá o povolení k přístupu ke GPS. Pokud není povolen zobrazí se mapa na poslední nebo výchozí pozici.
| __ALT SCÉNÁŘ 2__ | Uživatel zmáčkne tlačítko _zobrazit vlastní pozici_ na obrazovce (nebo v _rychlém menu_) s mapou.

## Odeslání hlášení
|  |  |
| - | - |
| __JMÉNO__ | Odeslání hlášení
| __DŮLEŽITOST__ | 5
| __AKTÉR__ | Voják v terénu
| __PRECONDITIONS__ | Zařízení s aplikací je připojené k síti (ethernet, data).
| __POSTCONDITIONS__ | Zpráva zůstává v chatroomu (historie).
| __SCÉNÁŘ__ | Uživatel v _rychlém menu_ zvolí _odeslat hlášení_. Zobrazí se chatroom s polem pro zadání zprávy.
| __ALT SCÉNÁŘ 1__ | Uživatel v _hlavním menu_ zvolí _kontakty_ a vybere odpovídající kontakt štábu. Zobrazí se chatroom s polem pro zadání zprávy.

## Zadání bodu na mapě
|  |  |
| - | - |
| __JMÉNO__ | Zadat bod (značení nepřítele, cíle, důležitého objektu nebo jiné informace)
| __DŮLEŽITOST__ | 5
| __AKTÉR__ | Voják v terénu, operátor ve štábu
| __PRECONDITIONS__ | Žádné, pro sdílení je nutné připojení k síti.
| __POSTCONDITIONS__ | Na mapě je na vybrané lokalitě zobrazen bod s danými vlastnostmi.
| __SCÉNÁŘ__ | Uživatel nastaví nastavý zaměřovač na mapě (střed obrazovky na požadovanou polohu) a v _rychlém menu_ zvolí _vytvořit bod_. Zobrazí se formulář tvorby bodu. Uživatel vybere buď šablonu nebo zadá požadované informace. Uživatel zmáčkne tlačítko _uložit_.

## Zjištění polohy ostatních členů týmu
|  |  |
| - | - |
| __JMÉNO__ | Zjištění polohy ostatních zařízení
| __DŮLEŽITOST__ | 5
| __AKTÉR__ | Voják v terénu
| __PRECONDITIONS__ | Zařízení s aplikací je připojené k síti. Existují další zařízení s aplikací, které jsou připojené k síti a mají zapnuté GPS.
| __POSTCONDITIONS__ | Žádné
| __SCÉNÁŘ__ | Uživatel si zobrazí mapu a vyhledá v ní symboly ostatních členů týmu. Symboly ostatních členů týmu budou mít speciální značení.

## Zadání plochy na mapě
|  |  |
| - | - |
| __JMÉNO__ | Zadat plochu (značení zóny nebezpečí, palby, pod kontrolou nebo jiné)
| __DŮLEŽITOST__ | 3
| __AKTÉR__ | Voják v terénu, operátor ve štábu
| __PRECONDITIONS__ | Žádné, pro sdílení je nutné připojení k síti.
| __POSTCONDITIONS__ | Na mapě je na vybrané lokalitě zobrazena plocha s danmi vlastnostmi.
| __SCÉNÁŘ__ | Uživatel v _rychlém menu_ zvolí _vytvořit plochu_. Nastaví zaměřovač na mapě na požadované místo a zvolí _přidat bod_. Takto pokračuje dokud nejsou zadané všechny hranové body plochy. Pro dokončení zmáčkně tlačítko _vytvořit plochu_. Při vytvoření se spojí první a poslední bod. Zobrazí se formulář s tvorbou plochy. Uživatel vybere buď šablonu nebo zadá požadované informace. Uživatel pak zmáčkně tlačítko _uložit_.

## Zadání cesty na mapě
|  |  |
| - | - |
| __JMÉNO__ | Zadat cestu (značení plánu přesunu a jiné)
| __DŮLEŽITOST__ | 3
| __AKTÉR__ | Voják v terénu, operátor ve štábu
| __PRECONDITIONS__ | Žádné, pro sdílení je nutné připojení k síti.
| __POSTCONDITIONS__ | Na mapě je na vybrané lokalitě zobrazena plocha s danmi vlastnostmi.
| __SCÉNÁŘ__ | Uživatel v _rychlém menu_ zvolí _vytvořit cestu_. Nastaví zaměřovač na mapě na požadované místo a zvolí _přidat bod_. Takto pokračuje dokud nejsou zadané všechny body cesty. Pro dokončení zmáčkně tlačítko _vytvořit cestu_. Zobrazí se formulář s tvorbou cesty. Uživatel vybere buď šablonu nebo zadá požadované informace. Uživatel pak zmáčkně tlačítko uložit.

## Odeslání rozkazu
|  |  |
| - | - |
| __JMÉNO__ | Vydání rozkazu
| __DŮLEŽITOST__ | 4
| __AKTÉR__ | Operátor ve štábu
| __PRECONDITIONS__ | Zařízení s aplikací je připojené k síti. Existuje další zařízení, které je připojené k síti.
| __POSTCONDITIONS__ | Zpráva zůstává v chatroomu (historie).
| __SCÉNÁŘ__ | Operátor ve štábním zařízení v _hlavním menu_ zvolí _kontakty_ a vyberé požadovaný kontakt. Zobrazí se chatroom s polem pro zadání a odeslání zprávy. Po odeslání se na cílovém zařízení zobrazí upozornění se zprávou.
| __ALT SCÉNÁŘ 1__ | Operátor ve štábním zařízení v _hlavním menu_ zvolí _kontakty_ a vybere možnost _odeslat všem_. Zobrazí se chatroom s polem pro zadání a odeslání zprávy. Po odeslání se na všech zařízení zobrazí upozornění se zprávou. Cílové zařízení nemohou na tuto zprávu odpovídat.

## Vzdálené vypnutí zařízení
|  |  |
| - | - |
| __JMÉNO__ | Vzdálené vypnutí
| __DŮLEŽITOST__ | 1
| __AKTÉR__ | Operátor ve štábu
| __PRECONDITIONS__ | Zařízení s aplikací je připojené k síti. Existuje další zařízení, které je připojené k síti.
| __POSTCONDITIONS__ | Vypnuté zařízení nemá přístup k datům.
| __SCÉNÁŘ__ | Operátor ve štábním zařízení v _hlavním menu_ zvolí _přerušit spojení_ a vyberé cílové zařízení. Vypnuté zařízení ztratí přístup k datům o poloze ostatních, bodům a plochám na mapě, kontakty a historii komunikace.

## Odeslání zprávy spolubojovníkovy
|  |  |
| - | - |
| __JMÉNO__ | Odeslání zprávy kolegovy
| __DŮLEŽITOST__ | 3
| __AKTÉR__ | Voják v terénu
| __PRECONDITIONS__ | Zařízení s aplikací je připojené k síti. Existuje další zařízení, které je připojené k síti.
| __POSTCONDITIONS__ | Zpráva zůstává v chatroomu (historie).
| __SCÉNÁŘ__ | Uživatel v _hlavním menu_ zvolí _kontakty_ a vybere požadovaný kontakt. Zobrazí se chatroom s polem pro zadání a odeslání zprávy. Po odeslání se na cílovém zařízení zobrazí upozornění.
| __ALT SCÉNÁŘ 1__ | Uživatel na mapě vyhledá značku požadovaného zařízení a klikne na ni. Zobrazí se chatroom s polem pro zadání a odeslání zprávy. Po odeslání se na cílovém zařízení zobrazí upozornění.
| __ALT SCÉNÁŘ 2__ | Uživatel v _hlavním menu_ zvolí _kontakty_ a vybere kontakt skupiny. Zobrazí se chatroom s polem pro zadání a odeslání zprávy. Po odeslání se na všech zařízení ve skupině zobrazí upozornění.

## Přidat fotografii k bodu/ploše
|  |  |
| - | - |
| __JMÉNO__ | Přidání fotografie
| __DŮLEŽITOST__ | 3
| __AKTÉR__ | Voják v terénu, operátor ve štábu
| __PRECONDITIONS__ | Zařízení má uloženou fotografii nebo má zapnutý fotoaparát. Existuje bod/plocha/cesta na mapě.
| __POSTCONDITIONS__ | Ve vybraném bodě/ploše/cestě je uložena fotografie.
| __SCÉNÁŘ__ | Uživatel zvolí požadovaný bod/plochu/cestu a v zobrazených detailních informacích zvolí _přidat fotku_. Otevře se aplikace fotoaparát. Po vyfocení se vrací zpět na detailní informace objektu a fotka se automaticky nahraje. Po přijmutí změn je fotka uložena v objektu.
| __ALT SCÉNÁŘ 1__ | Uživatel zvolí požadovaný objekt a v zobrazených detailních informacích zvolí _nahrát obrázek_. Zobrazí se souborový systém zařízení. Po zvolení souboru se vrací zpět na detailní informace objektu a obrázek se nahraje. Po přijmutí změn je obrázek uložen v objektu.
| __ALT SCÉNÁŘ 2__ | Po nahrání fotky/obrázku může uživatel vyfotit/nahrát další. Při uložení se do objektu uloží všechny soubory.
| __ALT SCÉNÁŘ 3__ | Po nahrání fotky/obrázku může uživatel soubor zrušit a do objektu ho nenahrávat.
