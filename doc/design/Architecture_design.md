# Návrh architektury
Tento soubor obsahuje návrh architektury aplikace. Je zde popsáno rozdělení aplikace na moduly, zodpovědnosti jednotlivých modulů a způsob vzájemné komunikace mezi moduly.

## Server
Server se bude starat o ukládání dat a správu akcí/__sezení__. Bude zpostředkovat veškerou komunikaci mezi uživately. Veškerá komunikace se serverem bude probíhat skrze HTTP požadavky nebo přes aplikaci pomocí. V rámci projektu bude dodán exportovaný seznam požadavků do aplikace Postman. Struktura serveru bude následující:

- `SessionController` (ovladač sezení) - Spravuje uživatele a jejich práva. Spravuje jednotlivá sezení (akce) a přiřazení uživatelů do sezení. Poskytuje middleware pro ověření platnosti sezení a uživatelských práv v daném sezením. Tento middleware bude využívat většina dalších modulů pro zjištění kontextu (akce) klienta.
- `MapController` (ovladač mapy) - Představuje sklad online map. Nebude závislý na sezení (akci) klienta. Pouze poskytuje samotné mapy, nestará se o mapové objekty. Z pohledu aplikace bude tato část read-only, přidávání map bude prováděno manuálně v databázi nebo pomocí api požadavků.
- `LocationController` (ovladač lokací) - Spravuje veškeré značení a objekty na mapě (body, plochy a cesty, pozice klientů). Je závislé na sezení, čtení i zápis musí být v platném sezení. Klient s tímto modulem pravidelně komunikuje pro aktualizaci údajů značení. Některé, často využívané, endpointy nebudou implementovány jako HTTP endpoint, ale jako přímé spojení.
- `MessageController` (ovladač zpráv) - Spravuje veškerou textovou komunikaci mezi klienty. Závisí na sezení, čtení i zápis musí být v platném sezení. Zprávy mezi klienty se přeposílají přes tento modul. Také je zde uložena historie komunikace. Bude implementovat jak Push tak Pull notifikace o nové zprávě.
- `TemplateController` (ovladač šablon) - Obsahuje uložiště šablon zpráv a mapových objektů. Samotné šablony nebudou závislé na sezení klienta, ale je u nich poznačeno, v kterém sezení byly vytvořeny. Obsahuje také záznamy dostupných šablon v daném sezení.

Schéma komunikace klienta se serverem bude poté vypadat následovně:

1. Klientský požadavek (obsahuje popis požadavku, identifikátor klienta a identifikátor sezení).
2. API - Ověří validitu požadavku, identifikuje klienta a sezení. Dále přeposílá požadavek na zodpovědný modul.
3. `SessionController`/`MapController`/`LocationController`/`MessageController`/`TemplateController` - Zpracují samotný požadavek v kontextu daného sezení a právy uživatele.
4. Zodpovědný modul vrátí odpověď klientovy.

Moduly serveru budou vnitřně implementovány ve třech vrstvách - API vrstva, Business vrsta a Datová vrstva:
- API vrstva přijímá požadavek, provádí kontrolu platnosti požadavku a autentizaci klienta. Bude implementována jako __RESTové rozhraní__.
- Business vrstva provádí konkrétní aplikační operace.
- Datová vrstva zajišťuje pouze uložení a načítání dat. Bude obsahovat specializované metody pro potřeby Business vrstvy. Bude implementována pomocí __Entity Frameworku__.

## Klient
Klient představuje mobilní aplikaci, se kterou budou pracovat uživatelé. Jeho struktura bude z větší části odpovídat architektuře serveru, se kterým bude komunikovat. Struktura klienta bude následující:

- `AppShell` - Výchozí bod aplikace. Stará se registraci ostatních stránek do aplikace. Provádí úvodní inicializaci (získání nastavení, identifikace klienta a aktivního sezení, ...). Poskytuje pouze technické a pomocné funkce, ne aplikační.
- `MapView` (pohled mapa) - Výchozí pohled aplikace. Stará se o získání a zobrazení mapy. Získává a vykresluje mapové objekty. Poskytuje formuláře pro vytváření nových mapových objektů.
- `MenuView` (pohled hlavní menu) - Slouží pouze jako rozcestí pro další stránky aplikace. Neposkytuje žádnou další funkcionalitu.
- `SettingsViews` (pohledy nastavení) - Starají se o zobrazení různých nastavení. Při změně nastavení upozorní moduly, kterých se to týká. Budou pouze provádět zápisy/čtení z uložiště nastavení, nebudou nijak ovlivňovat chování aplikace (o to se postarají upozorněné moduly).
- `ChatMenuView` (pohled chatu) - TODO
- `ChatView` (pohled chatu) - TODO
- `UserViews` (pphled uživatelů) - TODO

- `SettingsService` (služba nastavení) - Poskytuje metody pro získání a zapsání aktuálních nastavení. 
- `MessagingService` (služba zpráv) - Poskytuje rozhraní pro posílání zpráv mezi pohledy aplikace.
- `NotificationService` (služba upozornění) - Poskytuje mechanismy pro zobrazení různých druhů upozornění v aplikaci.

Bude využit návrhový vzor __Model-View-ViewModel__. Každý view (pohled) bude navázán na právě jeden view-model. Protože takto může být view-model značně rozsáhlý, bude business funkcionalita oddělena do oddělených souborů (`managers` a `services`). Ve view-modelů bude pouze datové navázení (data-binding) a implementace klientských akcí, které budou vyvolávat metody manažerů a služeb.
