using MiniHelpDesk.Commands;
using MiniHelpDesk.Models;
using MiniHelpDesk.Queries;
using MiniHelpDesk.Services;

namespace MiniHelpDesk.Handlers;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    void Handle(TCommand command);
}

/*
klasa: AddTicketHandler

opis:
Obsługuje komendę dodania zgłoszenia i zapisuje je w repozytorium po walidacji.

pola / właściwości:
_repository, TicketRepository, repozytorium zgłoszeń
_validator, TicketValidator, walidator danych zgłoszenia
*/
public sealed class AddTicketHandler(TicketRepository repository, TicketValidator validator)
    : ICommandHandler<AddTicketCommand>
{
    private readonly TicketRepository _repository = repository;
    private readonly TicketValidator _validator = validator;

    /*
    funkcja: Handle

    opis:
    Sprawdza dane komendy i dodaje poprawne zgłoszenie ze statusem Nowe.

    parametry:
    command, AddTicketCommand, dane nowego zgłoszenia

    zwraca:
    void, nic nie zwraca
    */
    public void Handle(AddTicketCommand command)
    {
        var errors = _validator.Validate(command.Name, command.Description, command.Priority);
        if (errors.Count > 0)
            throw new ArgumentException(string.Join(" ", errors.Values));

        _repository.Add(new Ticket(command.Name.Trim(), command.Description.Trim(), command.Priority));
    }
}

/*
klasa: DeleteTicketHandler

opis:
Obsługuje komendę usunięcia zgłoszenia.

pola / właściwości:
_repository, TicketRepository, repozytorium zgłoszeń
*/
public sealed class DeleteTicketHandler(TicketRepository repository) : ICommandHandler<DeleteTicketCommand>
{
    private readonly TicketRepository _repository = repository;

    /*
    funkcja: Handle

    opis:
    Usuwa zgłoszenie wskazane przez identyfikator.

    parametry:
    command, DeleteTicketCommand, komenda zawierająca identyfikator

    zwraca:
    void, nic nie zwraca
    */
    public void Handle(DeleteTicketCommand command)
    {
        if (!_repository.Remove(command.Id))
            throw new InvalidOperationException("Nie znaleziono zgłoszenia.");
    }
}

/*
klasa: ChangeStatusHandler

opis:
Obsługuje zmianę statusu istniejącego zgłoszenia.

pola / właściwości:
_repository, TicketRepository, repozytorium zgłoszeń
*/
public sealed class ChangeStatusHandler(TicketRepository repository) : ICommandHandler<ChangeStatusCommand>
{
    private readonly TicketRepository _repository = repository;

    /*
    funkcja: Handle

    opis:
    Zmienia status zgłoszenia. Setter modelu uruchamia powiadomienie PropertyChanged.

    parametry:
    command, ChangeStatusCommand, identyfikator i nowy status

    zwraca:
    void, nic nie zwraca
    */
    public void Handle(ChangeStatusCommand command)
    {
        if (command.Status is not ("Nowe" or "W trakcie" or "Zamknięte"))
            throw new ArgumentException("Nieprawidłowy status.");

        var ticket = _repository.FindById(command.Id)
            ?? throw new InvalidOperationException("Nie znaleziono zgłoszenia.");
        ticket.Status = command.Status;
    }
}

/*
klasa: GetTicketsHandler
opis: Obsługuje zapytanie o pobranie wszystkich zgłoszeń z repozytorium.
pola / właściwości:
repository, TicketRepository, repozytorium przechowujące zgłoszenia
*/
public sealed class GetTicketsHandler(TicketRepository repository)
{
    /*
    funkcja: Handle
    opis: Pobiera z repozytorium listę wszystkich zgłoszeń.
    parametry: query, GetTicketsQuery, zapytanie o listę zgłoszeń
    zwraca: IReadOnlyList<Ticket>, lista zgłoszeń tylko do odczytu
    */
    public IReadOnlyList<Ticket> Handle(GetTicketsQuery query) => repository.GetAll();
}
