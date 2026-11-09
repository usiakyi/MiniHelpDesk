using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MiniHelpDesk.Commands;
using MiniHelpDesk.Models;
using MiniHelpDesk.Queries;
using MiniHelpDesk.Services;

namespace MiniHelpDesk.ViewModels;

/*
klasa: MainViewModel

opis:
Łączy widok z logiką aplikacji. Obsługuje formularz, listę zgłoszeń, liczniki, walidację i komendy użytkownika.

pola / właściwości:
Tickets, ObservableCollection<Ticket>, obserwowalna lista zgłoszeń
SelectedTicket, Ticket?, aktualnie wybrane zgłoszenie
Name, string, wpisywana nazwa
Description, string, wpisywany opis
SelectedPriority, string?, wybrany priorytet
SelectedStatus, string, status wybrany do ustawienia
TotalCount, int, liczba wszystkich zgłoszeń
OpenCount, int, liczba zgłoszeń niezamkniętych
NameError, string, komunikat walidacji nazwy
DescriptionError, string, komunikat walidacji opisu
PriorityError, string, komunikat walidacji priorytetu
Message, string, komunikat operacji
*/
public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly TicketRepository _repository;
    private readonly TicketValidator _validator;
    private readonly CommandBus _bus;

    private string _name = "";
    private string _description = "";
    private string? _selectedPriority;
    private string _selectedStatus = "W trakcie";
    private Ticket? _selectedTicket;
    private string _nameError = "";
    private string _descriptionError = "";
    private string _priorityError = "";
    private string _message = "";

    public ObservableCollection<Ticket> Tickets { get; } = [];
    public string[] Priorities => TicketValidator.Priorities;
    public string[] Statuses { get; } = ["Nowe", "W trakcie", "Zamknięte"];

    public string Name { get => _name; set { Set(ref _name, value); Validate(); } }
    public string Description { get => _description; set { Set(ref _description, value); Validate(); } }
    public string? SelectedPriority { get => _selectedPriority; set { Set(ref _selectedPriority, value); Validate(); } }
    public string SelectedStatus { get => _selectedStatus; set => Set(ref _selectedStatus, value); }
    public Ticket? SelectedTicket { get => _selectedTicket; set => Set(ref _selectedTicket, value); }
    public string NameError { get => _nameError; private set => Set(ref _nameError, value); }
    public string DescriptionError { get => _descriptionError; private set => Set(ref _descriptionError, value); }
    public string PriorityError { get => _priorityError; private set => Set(ref _priorityError, value); }
    public string Message { get => _message; private set => Set(ref _message, value); }
    public int TotalCount => Tickets.Count;
    public int OpenCount => Tickets.Count(t => t.Status != "Zamknięte");

public System.Windows.Input.ICommand AddCommand { get; }
public System.Windows.Input.ICommand DeleteCommand { get; }
public System.Windows.Input.ICommand ChangeStatusCommand { get; }
public System.Windows.Input.ICommand RunTestsCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel(TicketRepository repository, TicketValidator validator, CommandBus bus)
    {
        _repository = repository;
        _validator = validator;
        _bus = bus;

        AddCommand = new Command(AddTicket);
        DeleteCommand = new Command(DeleteTicket);
        ChangeStatusCommand = new Command(ChangeStatus);
        RunTestsCommand = new Command(() => Message = MiniHelpDesk.Tests.TicketTests.RunAll());

        Refresh();
        Tickets.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(OpenCount));
        };
        foreach (var ticket in Tickets)
            ticket.PropertyChanged += (_, _) => OnPropertyChanged(nameof(OpenCount));
    }

    /*
    funkcja: AddTicket

    opis:
    Waliduje formularz, wysyła komendę dodania i odświeża obserwowalną listę.

    parametry:
    brak

    zwraca:
    void, nic nie zwraca
    */
    public void AddTicket()
    {
        var errors = _validator.Validate(Name, Description, SelectedPriority);
        ShowErrors(errors);
        if (errors.Count > 0)
        {
            Message = "Popraw dane formularza.";
            return;
        }

        try
        {
            _bus.Send(new AddTicketCommand(Name, Description, SelectedPriority!));
            Name = "";
            Description = "";
            SelectedPriority = null;
            Refresh();
            Message = "Zgłoszenie zostało dodane.";
        }
        catch (Exception ex) { Message = ex.Message; }
    }

    /*
    funkcja: DeleteTicket

    opis:
    Usuwa zaznaczone zgłoszenie przez CommandBus.

    parametry:
    brak

    zwraca:
    void, nic nie zwraca
    */
    public void DeleteTicket()
    {
        if (SelectedTicket is null) return;
        try
        {
            var id = SelectedTicket.Id;
            _bus.Send(new DeleteTicketCommand(id));
            Tickets.Remove(SelectedTicket);
            SelectedTicket = null;
            Message = "Zgłoszenie zostało usunięte.";
        }
        catch (Exception ex) { Message = ex.Message; }
    }

    /*
    funkcja: ChangeStatus

    opis:
    Zmienia status zaznaczonego zgłoszenia bez ponownego tworzenia listy.

    parametry:
    brak

    zwraca:
    void, nic nie zwraca
    */
    public void ChangeStatus()
    {
        if (SelectedTicket is null) return;
        try
        {
            _bus.Send(new Commands.ChangeStatusCommand(SelectedTicket.Id, SelectedStatus));
            OnPropertyChanged(nameof(OpenCount));
            Message = "Status został zmieniony.";
        }
        catch (Exception ex) { Message = ex.Message; }
    }

    /*
    funkcja: Refresh
    opis: Pobiera zgłoszenia z Query i aktualizuje kolekcję wyświetlaną w widoku.
    parametry: brak
    zwraca: void, nic nie zwraca
    */
    private void Refresh()
    {
        var query = new GetTicketsQuery();
        var result = new Handlers.GetTicketsHandler(_repository).Handle(query);
        Tickets.Clear();
        foreach (var ticket in result)
        {
            ticket.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(Ticket.Status))
                    OnPropertyChanged(nameof(OpenCount));
            };
            Tickets.Add(ticket);
        }
    }

    /*
    funkcja: Validate
    opis: Sprawdza aktualne wartości formularza i odświeża komunikaty walidacji.
    parametry: brak
    zwraca: void, nic nie zwraca
    */
    private void Validate() => ShowErrors(_validator.Validate(Name, Description, SelectedPriority));

    private void ShowErrors(Dictionary<string, string> errors)
    {
        NameError = errors.GetValueOrDefault("Name", "");
        DescriptionError = errors.GetValueOrDefault("Description", "");
        PriorityError = errors.GetValueOrDefault("Priority", "");
    }

    private bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
