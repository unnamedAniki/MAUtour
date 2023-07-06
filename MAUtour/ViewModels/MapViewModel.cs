using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUtour.ViewModels
{
    internal class MapViewModel : INotifyPropertyChanged
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
        public event PropertyChangedEventHandler PropertyChanged;
        public MapViewModel()
        {
            SearchText = "Поиск...";
            SearchPlaceholder = "Введите текст для поиска...";
            ShowButtonName = "Показать больше";
            HideButtonName = "Скрыть";
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

        private void OnPropertyChanged([CallerMemberName] string property = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
