# Návrh architektury
Tento soubor obsahuje návrh architektury aplikace. Je zde popsáno rozdělení aplikace na moduly, zodpovědnosti jednotlivých modulů a způsob vzájemné komunikace mezi moduly.

## Server
Server se bude starat o ukládání dat a správu akcí/__sezení__. Bude zpostředkovat veškerou komunikaci mezi uživately. Uživatel nebude se serverem nijak přímo pracovat, nebude dostupné žádné pracovní rozhraní. Veškerá komunikace se serverem bude probíhat skrze klientskou aplikaci. Struktura serveru bude následující:

- `SessionManager` (správce sezení) - Spravuje jednotlivá sezení (akce) a práve uživatelé. Představuje jediný přístupový bod do serveru pro klienta. Klientské požadavky na speciální funkce předává dalším modulům.
- `MapRepository` (repositář map) - Představuje sklad online map. Není závislý na sezení (akci) klienta. Pouze poskytuje samotné mapy, nestará se o mapové objekty.
- `LocationManager` (správce lokací) - Spravuje veškeré značení a objekty na mapě (body, plochy a cesty, pozice klientů). Závisí na sezení. Klient s tímto modulem pravidelně komunikuje pro aktualizaci údajů značení.
- `ChatManager` (správce komunikace) - Spravuje veškerou komunikaci mezi klienty. Závisí na sezení. Zprávy mezi klienty se přeposílají a ukládají na tomto modulu. Při odeslání nové zprávy notifikuje příslušné klienty.
- `TemplateRepository` (repositář šablon) - Obsahuje uložiště šablon zpráv a mapových objektů. Samotné šablony nejsou závislé na sezení klienta, ale je u nich poznačení v kterém sezení byly vytvořeny. Obsahuje také záznam dostupných šablon v daném sezení.

Schéma komunikace klienta se serverem bude poté vypadat následovně:

1. Klientský požadavek (obsahuje popis požadavku, identifikátor klienta a identifikátor sezení).
2. `SessionManagerAPI` - Ověří validitu požadavku, identifikuje klienta a sezení. Dále přeposílá požadavek na zodpovědný modul.
3. `SessionManager`/`MapRepository`/`LocationManager`/`CharManager`/`TemplateRepository` - Zpracují samotný požadavek v kontextu daného sezení a právy uživatele.
4. Zodpovědný modul vrátí odpověď `SessionManagerAPI`, který odešle odpověď klientovy.

## Klient
Klient představuje mobilní aplikaci, se kterou budou pracovat uživatelé. Jeho struktura bude následující:
