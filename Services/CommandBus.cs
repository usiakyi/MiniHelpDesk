using MiniHelpDesk.Commands;
using MiniHelpDesk.Handlers;

namespace MiniHelpDesk.Services;

/*
klasa: CommandBus

opis:
Prosty mechanizm CQRS przekazujący komendy do odpowiednich handlerów.

pola / właściwości:
_repository, TicketRepository, repozytorium używane przez handlery
_validator, TicketValidator, walidator danych
*/
public sealed class CommandBus(TicketRepository repository, TicketValidator validator)
{
    private readonly TicketRepository _repository = repository;
    private readonly TicketValidator _validator = validator;

    /*
    funkcja: Send

    opis:
    Rozpoznaje typ komendy i uruchamia właściwy handler.

    parametry:
    command, ICommand, komenda do wykonania

    zwraca:
    void, nic nie zwraca
    */
    public void Send(ICommand command)
    {
        switch (command)
        {
            case AddTicketCommand add:
                new AddTicketHandler(_repository, _validator).Handle(add);
                break;
            case DeleteTicketCommand delete:
                new DeleteTicketHandler(_repository).Handle(delete);
                break;
            case ChangeStatusCommand status:
                new ChangeStatusHandler(_repository).Handle(status);
                break;
            default:
                throw new NotSupportedException($"Nieobsługiwana komenda: {command.GetType().Name}");
        }
    }
}
