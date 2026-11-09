using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MiniHelpDesk.Models;

/*
klasa: Ticket

opis:
Przechowuje dane pojedynczego zgłoszenia technicznego i powiadamia interfejs o zmianach właściwości.

pola / właściwości:
Id, Guid, unikalny identyfikator zgłoszenia
Name, string, nazwa problemu
Description, string, opis problemu
Priority, string, priorytet zgłoszenia
Status, string, aktualny status zgłoszenia
*/
public class Ticket : INotifyPropertyChanged
{
    private string _name;
    private string _description;
    private string _priority;
    private string _status;

    public Guid Id { get; }
    public string Name { get => _name; set => SetField(ref _name, value); }
    public string Description { get => _description; set => SetField(ref _description, value); }
    public string Priority { get => _priority; set => SetField(ref _priority, value); }
    public string Status { get => _status; set => SetField(ref _status, value); }

    public event PropertyChangedEventHandler? PropertyChanged;

    /*
    funkcja: Ticket
    opis: Tworzy zgłoszenie z unikalnym identyfikatorem i statusem początkowym.
    parametry:
    name, string, nazwa zgłoszenia
    description, string, opis zgłoszenia
    priority, string, priorytet zgłoszenia
    status, string, status zgłoszenia; domyślnie Nowe
    zwraca: void, nic nie zwraca
    */
    public Ticket(string name, string description, string priority, string status = "Nowe")
    {
        Id = Guid.NewGuid();
        _name = name;
        _description = description;
        _priority = priority;
        _status = status;
    }

    /*
    funkcja: SetField

    opis:
    Ustawia nową wartość pola i zgłasza zmianę właściwości, jeżeli wartość uległa zmianie.

    parametry:
    field, T, referencja do pola przechowującego wartość
    value, T, nowa wartość pola
    propertyName, string, nazwa właściwości wywołującej metodę

    zwraca:
    bool, informacja czy wartość została zmieniona
    */
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
