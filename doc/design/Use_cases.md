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
| __ALT SCÉNÁŘ 2__ | Uživatel zmáčkne tlačítko _zobrazit vlastní pozici_ na obrazovce s mapou.

## Odeslání hlášení
|  |  |
| - | - |
| __JMÉNO__ | Odeslání hlášení
| __DŮLEŽITOST__ | 5
| __AKTÉR__ | Voják v terénu
| __PRECONDITIONS__ | Zařízení s aplikací je připojené k síti (ethernet, data).
| __POSTCONDITIONS__ | Žádné
| __SCÉNÁŘ__ | Uživatel v _rychlém menu_ zvolí _odeslat hlášení_. Zobrazí se chatroom s polem pro zadání zprávy.
| __ALT SCÉNÁŘ 1__ | Uživatel v _hlavním menu_ zvolí _kontakty_ a vybere odpovídající kontaktu štábu. Zobrazí se chatroom s polem pro zadání zprávy.

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
| __SCÉNÁŘ__ | Uživatel si zobrazí mapu a vyhledá v ní symboly ostatních členů týmu.

## Zadání plochy na mapě
|  |  |
| - | - |
| __JMÉNO__ | Zadat plochu (značení zóny nebezpečí, palby, pod kontrolou nebo jiné)
| __DŮLEŽITOST__ | 3
| __AKTÉR__ | Voják v terénu, operátor ve štábu
| __PRECONDITIONS__ | Žádné, pro sdílení je nutné připojení k síti.
| __POSTCONDITIONS__ | Na mapě je na vybrané lokalitě zobrazena plocha s danmi vlastnostmi.
| __SCÉNÁŘ__ | Uživatel v _rychlém menu_ zvolí _vytvořit plochu_. Nastaví zaměřovač na mapě na požadovanou plochu a zvolí _přidat bod_. Takto pokračuje dokud nejsou zadané všechny hranové body plochy. Pro dokončení zmáčkně tlačítko _vytvořit plochu_. Při vytvoření se spojí první a poslední bod. Zobrazí se formulář s tvorbou plochy. Uživatel vybere buď šablonu nebo zadá požadované informace. Uživatel zmáčkně tlačítko _uložit_.

## Zadání cesty na mapě
|  |  |
| - | - |
| __JMÉNO__ | Zadat cestu (značení plánu přesunu a jiné)
| __DŮLEŽITOST__ | 3
| __AKTÉR__ | Voják v terénu, operátor ve štábu
| __PRECONDITIONS__ | Žádné, pro sdílení je nutné připojení k síti.
| __POSTCONDITIONS__ | Na mapě je na vybrané lokalitě zobrazena plocha s danmi vlastnostmi.
| __SCÉNÁŘ__ | Uživatel v _rychlém menu_ zvolí _vytvořit cestu_. Nastaví zaměřovač na mapě na požadovanou plochu a zvolí _přidat bod_. Takto pokračuje dokud nejsou zadané všechny body cesty. Pro dokončení zmáčkně tlačítko _vytvořit cestu_. Zobrazí se formulář s tvorbou cesty. Uživatel vybere buď šablonu nebo zadá požadované informace. Uživatel zmáčkně tlačítko uložit.

