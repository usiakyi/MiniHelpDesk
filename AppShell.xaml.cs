using MiniHelpDesk.Views;

namespace MiniHelpDesk;

public partial class AppShell : Shell
{
    public AppShell(MainPage page)
    {
        InitializeComponent();
        Items.Clear();
        Items.Add(new ShellContent { Title = "Mini HelpDesk", Content = page });
    }
}
