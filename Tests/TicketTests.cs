using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniHelpDesk.Commands;
using MiniHelpDesk.Models;
using MiniHelpDesk.Services;

namespace MiniHelpDesk.Tests;

/*
klasa: TicketTests

opis:
Zawiera testy jednostkowe logiki walidacji, repozytorium, komend oraz powiadomień Observable.

pola / właściwości:
brak
*/
[TestClass]
public class TicketTests
{
    [TestMethod]
    public void ValidTicketPassesValidation()
    {
        var validator = new TicketValidator();
        Assert.AreEqual(0, validator.Validate("Drukarka", "Nie działa drukarka", "Wysoki").Count);
    }

    [TestMethod]
    public void ShortNameIsRejected()
    {
        var validator = new TicketValidator();
        Assert.IsTrue(validator.Validate("ab", "Nie działa drukarka", "Niski").ContainsKey("Name"));
    }

    [TestMethod]
    public void ShortDescriptionIsRejected()
    {
        var validator = new TicketValidator();
        Assert.IsTrue(validator.Validate("Drukarka", "Problem", "Średni").ContainsKey("Description"));
    }

    [TestMethod]
    public void AddCommandStoresTicketWithNewStatus()
    {
        var repository = new TicketRepository();
        var bus = new CommandBus(repository, new TicketValidator());
        bus.Send(new AddTicketCommand("Drukarka", "Nie działa drukarka", "Wysoki"));
        Assert.AreEqual(1, repository.GetAll().Count);
        Assert.AreEqual("Nowe", repository.GetAll()[0].Status);
    }

    [TestMethod]
    public void StatusChangeRaisesPropertyChanged()
    {
        var ticket = new Ticket("Drukarka", "Nie działa drukarka", "Niski");
        var changed = false;
        ticket.PropertyChanged += (_, e) => changed = e.PropertyName == nameof(Ticket.Status);
        ticket.Status = "W trakcie";
        Assert.IsTrue(changed);
    }

    public static string RunAll()
    {
        var tests = new Action[]
        {
            new TicketTests().ValidTicketPassesValidation,
            new TicketTests().ShortNameIsRejected,
            new TicketTests().ShortDescriptionIsRejected,
            new TicketTests().AddCommandStoresTicketWithNewStatus,
            new TicketTests().StatusChangeRaisesPropertyChanged
        };

        var passed = 0;
        var failures = new List<string>();
        foreach (var test in tests)
        {
            try { test(); passed++; }
            catch (Exception ex) { failures.Add(ex.Message); }
        }
        return $"Wynik: {passed}/{tests.Length} testów zaliczonych." +
               (failures.Count == 0 ? "" : Environment.NewLine + string.Join(Environment.NewLine, failures));
    }
}
