using Microsoft.Extensions.DependencyInjection;
using MiniHelpDesk.Views;

namespace MiniHelpDesk;

public partial class App : Application
{
    public App(IServiceProvider services)
    {
        InitializeComponent();
        MainPage = new AppShell(services.GetRequiredService<MainPage>());
    }
}
