using System.Windows.Input;
using CommunityToolkit.Maui.Views;

using MAUtour.Views.Dialogs;

namespace MAUtour.ViewModels
{
    internal class MapViewModel : BaseViewModel
    {
        private string _searchText;
        private string _searchPlaceholder;
        private string _showButtonName;
        private string _hideButtonName;
        private string _pinName;
        private string _pinDescription;
        private string _pinInfo;
        private string _roadLabel;
        private string _disableModeButtonText;
        private string _addPinButtonText;
        public MapViewModel()
        {
            SearchText = "Поиск...";
            SearchPlaceholder = "Введите текст для поиска...";
            ShowButtonName = "Показать больше";
            HideButtonName = "Скрыть";
            PinName = string.Empty;
            PinDescription = string.Empty;
            PinInfo = string.Empty;
            AddPinButtonText = "Добавить метку";
            DisableModeButtonText = "Отменить создание маршрута";
        }
        public ICommand ShowDialog { get; private set; }
        public string SearchText
        {
            get => _searchText;
            set 
            { 
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public string SearchPlaceholder
        {
            get => _searchPlaceholder; 
            set => _searchPlaceholder = value; 
        }

        public string ShowButtonName
        {
            get => _showButtonName;
            set => _showButtonName = value;
        }

        public string HideButtonName
        {
            get => _hideButtonName; 
            set => _hideButtonName = value;
        }

        public string PinName
        {
            get => _pinName; 
            set
            {
                _pinName = value;
                OnPropertyChanged();
            }
        }

        public string PinDescription
        {
            get => _pinDescription; 
            set
            {
                _pinDescription = value;
                OnPropertyChanged();
            }
        }

        public string PinInfo
        {
            get => _pinInfo; 
            set => _pinInfo = value;   
        }

        public string AddPinButtonText
        {
            get => _addPinButtonText;
            set => _addPinButtonText = value;
        }

        public string RoadLabel
        {
            get => _roadLabel;
            set => _roadLabel = value;
        }

        public string DisableModeButtonText
        {
            get => _disableModeButtonText; 
            set => _disableModeButtonText = value;
        }
    }
}
