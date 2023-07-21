using CommunityToolkit.Maui.Views;

using Mapsui;

using MAUtour.Local.DBConnect;
using MAUtour.Local.UnitOfWork.Interface;
using MAUtour.ViewModels.Dialogs;

namespace MAUtour.Views.Dialogs;

public partial class AddPinDialog : Popup
{
	public AddPinDialog(bool isPin, IUnitOfWork unitOfWork, MPoint position)
	{
		this.BindingContext = new DialogViewModel(this, unitOfWork, position, isPin);
		InitializeComponent();
	}

    private void Close(object sender, EventArgs e)
    {
		this.Close(false);
    }
}