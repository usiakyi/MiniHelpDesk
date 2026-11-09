using MiniHelpDesk.Models;

namespace MiniHelpDesk.Services;

/*
klasa: TicketRepository

opis:
Przechowuje zgłoszenia w pamięci i udostępnia podstawowe operacje na kolekcji.

pola / właściwości:
_tickets, List<Ticket>, wewnętrzna lista zgłoszeń
*/
public class TicketRepository
{
    private readonly List<Ticket> _tickets = [];

    /*
    funkcja: GetAll

    opis:
    Zwraca kopię listy wszystkich zgłoszeń.

    parametry:
    brak

    zwraca:
    IReadOnlyList<Ticket>, lista zgłoszeń
    */
    public IReadOnlyList<Ticket> GetAll() => _tickets.ToList();

    /*
    funkcja: Add

    opis:
    Dodaje zgłoszenie do repozytorium.

    parametry:
    ticket, Ticket, zgłoszenie do dodania

    zwraca:
    void, nic nie zwraca
    */
    public void Add(Ticket ticket) => _tickets.Add(ticket);

    /*
    funkcja: Remove

    opis:
    Usuwa zgłoszenie o podanym identyfikatorze.

    parametry:
    id, Guid, identyfikator zgłoszenia

    zwraca:
    bool, informacja czy zgłoszenie zostało usunięte
    */
    public bool Remove(Guid id)
    {
        var ticket = _tickets.FirstOrDefault(t => t.Id == id);
        return ticket is not null && _tickets.Remove(ticket);
    }

    /*
    funkcja: FindById

    opis:
    Wyszukuje zgłoszenie według identyfikatora.

    parametry:
    id, Guid, identyfikator zgłoszenia

    zwraca:
    Ticket?, znalezione zgłoszenie lub null
    */
    public Ticket? FindById(Guid id) => _tickets.FirstOrDefault(t => t.Id == id);
}
