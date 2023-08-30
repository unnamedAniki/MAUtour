using CommunityToolkit.Maui.Views;

using Mapsui;

using MAUtour.Local.UnitOfWork.Interface;
using MAUtour.Views.Dialogs;

using System.Windows.Input;

namespace MAUtour.ViewModels.Dialogs
{
    internal class ChooseActionViewModel : BaseViewModel
    {
        private IUnitOfWork _unitOfWork;
        private MPoint _position;
        private Page _page;
        public ChooseActionViewModel(Popup popup, Page page, IUnitOfWork unitOfWork, MPoint position)
        {
            _unitOfWork = unitOfWork;
            _position = position;
            _page = page;
            InitializeDialog(popup);
        }

        private void InitializeDialog(Popup popup)
        {
            Info = "Что вы хотите добавить на карту?";
            PinButtonTitle = "Добавить метку";
            RoadButtonTitle = "Добавить маршрут";
            ChoosePin = new Command(obj =>
            {
                popup.Close(true);
            });
            ChooseRoad = new Command(obj =>
            {
                popup.Close(false);
            });
        }

        public ICommand ChoosePin { get; private set; }
        public ICommand ChooseRoad { get; private set; }
        public string Info { get; private set; }
        public string PinButtonTitle { get; private set; }
        public string RoadButtonTitle { get; private set; }
    }
}
