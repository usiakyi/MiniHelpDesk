namespace MiniHelpDesk.Commands;

/*
klasa: ICommand
opis: Interfejs znacznikowy dla komend obsługiwanych przez CommandBus.
pola / właściwości: brak
*/
public interface ICommand { }

/*
klasa: AddTicketCommand
opis: Przenosi dane potrzebne do utworzenia nowego zgłoszenia.
pola / właściwości:
Name, string, nazwa nowego zgłoszenia
Description, string, opis nowego zgłoszenia
Priority, string, priorytet nowego zgłoszenia
*/
public sealed record AddTicketCommand(string Name, string Description, string Priority) : ICommand;

/*
klasa: DeleteTicketCommand
opis: Przenosi identyfikator zgłoszenia przeznaczonego do usunięcia.
pola / właściwości:
Id, Guid, identyfikator usuwanego zgłoszenia
*/
public sealed record DeleteTicketCommand(Guid Id) : ICommand;

/*
klasa: ChangeStatusCommand
opis: Przenosi identyfikator zgłoszenia i jego nowy status.
pola / właściwości:
Id, Guid, identyfikator zmienianego zgłoszenia
Status, string, nowy status zgłoszenia
*/
public sealed record ChangeStatusCommand(Guid Id, string Status) : ICommand;
