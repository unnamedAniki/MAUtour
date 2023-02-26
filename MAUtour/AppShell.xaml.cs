namespace MAUtour;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("pin/details", typeof(PinPage));
	}
}
