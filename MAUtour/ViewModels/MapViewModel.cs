using CommunityToolkit.Maui.Views;
using System.Windows.Input;

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
        private string _imageLabel;
        private string _endRoadButton;
        public MapViewModel()
        {
            SearchText = "Поиск...";
            SearchPlaceholder = "Введите текст для поиска...";
            ShowButtonName = "Показать больше";
            HideButtonName = "Скрыть";
            AddPinButtonText = "Добавить метку на маршруте";
            RoadLabel = "Включен режим создания маршрута";
            DisableModeButtonText = "Закончить создание";
            EndRoadButton = "Закрыть";
        }
        public string ImageLabel
        {
            get => _imageLabel;
            set => _imageLabel = value;
        }
        public string EndRoadButton
        {
            get => _endRoadButton;
            set => _endRoadButton = value;
        }
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
