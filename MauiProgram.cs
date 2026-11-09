using MiniHelpDesk.Services;
using MiniHelpDesk.ViewModels;
using MiniHelpDesk.Views;

namespace MiniHelpDesk;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddSingleton<TicketRepository>();
        builder.Services.AddSingleton<TicketValidator>();
        builder.Services.AddSingleton<CommandBus>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
