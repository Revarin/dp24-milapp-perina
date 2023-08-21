# Návrh architektury
Tento soubor obsahuje návrh architektury aplikace. Je zde popsáno rozdělení aplikace na moduly, zodpovědnosti jednotlivých modulů a způsob vzájemné komunikace mezi moduly.

## Server
Server se bude starat o ukládání dat a správu akcí/__sezení__. Bude zpostředkovat veškerou komunikaci mezi uživately. Server nebude mimo aplikaci poskytovat žádné rozhraní pro jeho správu. Veškerá komunikace se serverem bude probíhat skrze klientskou aplikaci. Struktura serveru bude následující:

- `SessionManager` (správce sezení) - Spravuje jednotlivá sezení (akce) a práva uživatelů. Obsahuje mechanismy pro autentizaci uživatele a sezení. Je využíván většinou dalších modulů pro ověření aktuálního sezení klienta.
- `MapRepository` (repositář map) - Představuje sklad online map. Není závislý na sezení (akci) klienta. Pouze poskytuje samotné mapy, nestará se o mapové objekty.
- `LocationManager` (správce lokací) - Spravuje veškeré značení a objekty na mapě (body, plochy a cesty, pozice klientů). Závisí na sezení. Klient s tímto modulem pravidelně komunikuje pro aktualizaci údajů značení.
- `ChatManager` (správce komunikace) - Spravuje veškerou komunikaci mezi klienty. Závisí na sezení. Zprávy mezi klienty se přeposílají přes tento modul. Také je zde uložena historie komunikace. Při odeslání nové zprávy notifikuje příslušné klienty.
- `TemplateRepository` (repositář šablon) - Obsahuje uložiště šablon zpráv a mapových objektů. Samotné šablony nejsou závislé na sezení klienta, ale je u nich poznačeno, v kterém sezení byly vytvořeny. Obsahuje také záznam dostupných šablon v daném sezení.

Schéma komunikace klienta se serverem bude poté vypadat následovně:

1. Klientský požadavek (obsahuje popis požadavku, identifikátor klienta a identifikátor sezení).
2. API - Ověří validitu požadavku, identifikuje klienta a sezení. Dále přeposílá požadavek na zodpovědný modul.
3. `SessionManager`/`MapRepository`/`LocationManager`/`CharManager`/`TemplateRepository` - Zpracují samotný požadavek v kontextu daného sezení a právy uživatele.
4. Zodpovědný modul vrátí odpověď `SessionManagerAPI`, který odešle odpověď klientovy.

Moduly serveru budou vnitřně implementovány ve třech vrstvách - API vrstva, Business vrsta a Datová vrstva:
- API vrstva přijímá požadavek, provádí kontrolu platnosti požadavku a autentizaci klienta. Bude implementována jako __RESTové rozhraní__.
- Business vrstva provádí konkrétní aplikační operace.
- Datová vrstva zajišťuje pouze uložení a načítání dat. Bude obsahovat specializované metody pro potřeby Business vrstvy. Bude implementována pomocí __Entity Frameworku__.

## Klient
Klient představuje mobilní aplikaci, se kterou budou pracovat uživatelé. Jeho struktura bude z větší části odpovídat architektuře serveru, se kterým bude komunikovat. Struktura klienta bude následující:

- `MapManager` (správce mapy) - Stará se o získání a zobrazení mapy. Získává a vykresluje mapové objekty. Poskytuje formuláře pro vytváření nových mapových objektů. Poskytuje formuláře pro vytváření nových šablon mapových značení.
- `ChatManager` (správce komunikace) - Stará se o komunikaci mezi uživately. Odesílá a získává zprávy v daném chatroomu. Zpracovává upozornění na novou zprávu od serveru. Poskytuje formuláře pro vytváření nových šablon zpráv.
- `UserManager` (správce uživatelů) - Poskytuje nástroje pro štáb pro správu uživatelů v sezení. Umožňuje nastavovat práva jednotlivých uživatelů. Umožňuje vytvářet skupiny uživatelé a přiřazovat jim různé vlastnosti a práva.
- `NotificationManager` (správce upozornění) - Poskytuje mechanismy pro zobrazení upozornění v aplikaci.
- `SettingsManager` (správce nastavení) - Obsahuje nastavení sezení, komunikace se serverem a další. Je využíván ostatními moduly.
