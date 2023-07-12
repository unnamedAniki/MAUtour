using CommunityToolkit.Maui.Views;

using MAUtour.ViewModels.Dialogs;

namespace MAUtour.Views.Dialogs;

public partial class AddPinDialog : Popup
{
	public AddPinDialog(bool isPin)
	{
		this.BindingContext = new DialogViewModel(this, isPin);
		InitializeComponent();
	}

    private void Close(object sender, EventArgs e)
    {
		this.Close();
    }
}