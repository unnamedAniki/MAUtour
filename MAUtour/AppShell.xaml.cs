using MAUtour.Views.Dialogs;

namespace MAUtour;

public partial class AppShell : Shell
{
	public AppShell()
    {
        Routing.RegisterRoute("pin/details", typeof(PinPage));
        Routing.RegisterRoute("MapPage", typeof(MapPage));
        InitializeComponent();
	}
}
