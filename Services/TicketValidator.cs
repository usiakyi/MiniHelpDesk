namespace MiniHelpDesk.Services;

/*
klasa: TicketValidator

opis:
Sprawdza poprawność danych zgłoszenia przed wykonaniem komendy dodawania.

pola / właściwości:
Priorities, string[], dozwolone wartości priorytetu
*/
public class TicketValidator
{
    public static readonly string[] Priorities = ["Niski", "Średni", "Wysoki"];

    /*
    funkcja: Validate

    opis:
    Waliduje nazwę, opis oraz priorytet zgłoszenia. Usuwa spacje z brzegów tekstu.

    parametry:
    name, string, nazwa zgłoszenia
    description, string, opis zgłoszenia
    priority, string, wybrany priorytet

    zwraca:
    Dictionary<string, string>, błędy przypisane do pól; pusty słownik oznacza poprawne dane
    */
    public Dictionary<string, string> Validate(string? name, string? description, string? priority)
    {
        var errors = new Dictionary<string, string>();
        var cleanName = name?.Trim() ?? "";
        var cleanDescription = description?.Trim() ?? "";

        if (cleanName.Length < 3)
            errors["Name"] = "Nazwa musi mieć co najmniej 3 znaki.";
        if (cleanDescription.Length < 10)
            errors["Description"] = "Opis musi mieć co najmniej 10 znaków.";
        if (string.IsNullOrWhiteSpace(priority) || !Priorities.Contains(priority))
            errors["Priority"] = "Wybierz priorytet.";

        return errors;
    }
}
