using CommunityToolkit.Maui.Views;
using Mapsui;
using MAUtour.Local.UnitOfWork.Interface;
using MAUtour.ViewModels.Dialogs;

namespace MAUtour.Views.Dialogs;

public partial class ChooseActionDialog : Popup
{
	public ChooseActionDialog(Page page, IUnitOfWork unitOfWork, MPoint position)
	{
        this.BindingContext = new ChooseActionViewModel(this, page, unitOfWork, position);
        InitializeComponent();
	}
}