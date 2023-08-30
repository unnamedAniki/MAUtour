using CommunityToolkit.Maui.Views;

using Mapsui;

using MAUtour.Local.UnitOfWork.Interface;
using MAUtour.ViewModels.Dialogs;

namespace MAUtour.Views.Dialogs;

public partial class AddRouteDialog : Popup
{
	public AddRouteDialog(IUnitOfWork unitOfWork)
	{
		this.BindingContext = new RouteDialogViewModel(this, unitOfWork);
		InitializeComponent();
	}

    private void Close(object sender, EventArgs e)
    {
        this.Close(null);
    }
}