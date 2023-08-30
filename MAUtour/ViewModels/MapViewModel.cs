using System.Collections.ObjectModel;
using System.Windows.Input;

using MAUtour.Local.Models;
using MAUtour.Local.UnitOfWork.Interface;

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
        private IUnitOfWork _unitOfWork;
        public ObservableCollection<Routes> Routes { get;  private set; }
        public MapViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeData();
        }

        private async void InitializeData()
        {
            Routes = new ObservableCollection<Routes>(await _unitOfWork.routesRepository.GetAllAsync());
            SearchText = "Поиск...";
            SearchPlaceholder = "Введите текст для поиска...";
            ShowButtonName = "Показать больше";
            HideButtonName = "Скрыть";
            PinName = string.Empty;
            PinDescription = string.Empty;
            PinInfo = string.Empty;
            AddPinButtonText = "Добавить метку";
            DisableModeButtonText = "Отменить создание маршрута";
            RoadLabel = "Режим создания маршрута";
            SaveRoadButtonText = "Сохранить маршрут";
            ShowRoadText = "Показать";
        }
        public string ShowRoadText { get; private set; }
        public ICommand ShowDialog { get; private set; }
        public ICommand ShowRoad { get; private set; }
        public string SaveRoadButtonText { get; private set; }
        public string RoadLabel { get; private set; }  
        public string SearchText
        {
            get => _searchText;
            set 
            { 
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public string SearchPlaceholder { get; private set; }
        public string ShowButtonName { get; private set; }
        public string HideButtonName { get; private set; }
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

        public string DisableModeButtonText
        {
            get => _disableModeButtonText; 
            set => _disableModeButtonText = value;
        }
    }
}
