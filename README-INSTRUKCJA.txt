MINI HELPDESK — GRUPA II — WINDOWS DESKTOP

Wymagania:
- Windows 11
- .NET 10 SDK
- .NET MAUI workload
- Visual Studio Code (lub Visual Studio)

Uruchomienie:
1. Rozpakuj folder MiniHelpDesk.
   Uwaga: wpis „10.0.100-manifests...” z workload list nie jest wersją SDK.
   Wersję SDK sprawdzisz osobno poleceniem: dotnet --version.
2. Otwórz folder zawierający MiniHelpDesk.csproj w VS Code.
3. W terminalu sprawdź:
   dotnet --version
   dotnet workload list
4. Przywróć pakiety:
   dotnet restore
5. Uruchom:
   dotnet build
   dotnet run -f net10.0-windows10.0.19041.0

Jeżeli `dotnet run` nie wybierze celu automatycznie, użyj:
   dotnet build -f net10.0-windows10.0.19041.0

Testy:
W aplikacji naciśnij przycisk „Uruchom testy MSTest”.
Testy używają asercji MSTest i własnego runnera uruchamianego z aplikacji.
To nie jest standardowe uruchamianie `dotnet test`.

WAŻNE:
Ten projekt jest przygotowany wyłącznie dla Windows zgodnie z adnotacją dla grupy II.
Dane są przechowywane w pamięci i znikają po zamknięciu aplikacji.
