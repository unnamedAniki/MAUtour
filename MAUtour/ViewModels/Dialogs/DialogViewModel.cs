using CommunityToolkit.Maui.Core;
using MAUtour.Local.DBConnect;
using CommunityToolkit.Maui.Views;

using MAUtour.Local.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MAUtour.Local.UnitOfWork.Interface;
using System.Collections.ObjectModel;
using MAUtour.Local.Models;
using Mapsui;

namespace MAUtour.ViewModels.Dialogs
{
    internal class DialogViewModel : BaseViewModel
    {
        private string _name;
        private string _description;
        private string _additionalInfo;
        private IUnitOfWork _unitOfWork;
        private MPoint _position;
        public ObservableCollection<PinTypes> PinTypes { get; set; } = new();
        public ObservableCollection<RouteTypes> RouteTypes { get; set; } = new();
        public PinTypes SelectedPinType { get; set; }
        public DialogViewModel(Popup popup, IUnitOfWork unitOfWork, MPoint position, bool isPin = false)
        {
            _unitOfWork = unitOfWork;
            _position = position;
            PinTypes = new ObservableCollection<PinTypes>(_unitOfWork.pinTypesRepository.GetAllAsync().Result);
            SelectedPinType = new PinTypes();
            RouteTypes = new ObservableCollection<RouteTypes>(_unitOfWork.routeTypesRepository.GetAllAsync().Result);
            InizializeDialog(isPin, popup);
        }

        private async void InizializeDialog(bool isPin, Popup popup)
        {
            NameLabel = "Наименование";
            TypeLabel = "Тип";
            DescriptionLabel = "Описание";
            AdditionalInfoLabel = "Комментарий";
            AddButtonText = "Добавить";
            CancelButtonText = "Отменить";
            Name = string.Empty;
            NamePlaceholder = "Введите наименование";
            Description = string.Empty;
            DescriptionPlaceholder = "Введите описание";
            AdditionalInfo = string.Empty;
            DescriptionPlaceholder = "Введите комментарий";
            if (isPin)
            {
                Title = "Добавление новой метки";
                Add = new Command(async (obj) =>
                {
                    var test = new Pins
                    {
                        PinTypeId = SelectedPinType.Id,
                        Name = Name,
                        Description = Description,
                        Latitude = _position.X,
                        Longitude = _position.Y,
                    };
                    await _unitOfWork.pinRepository.AddAsync(test);
                    await _unitOfWork.CommitAsync();
                    popup.Close(true);
                });
            }
            else
            {
                Title = "Добавление нового маршрута";
                Add = new Command(async (obj) =>
                {
                    return;
                });
            }
        }
        public ICommand Add { get; private set; }
        public ICommand Cancel { get; private set; }
        public string Title { get; private set; }
        public string NameLabel { get; private set; }
        public string TypeLabel { get; private set; }
        public string DescriptionLabel { get; private set; }
        public string AdditionalInfoLabel { get; private set; }
        public string NamePlaceholder { get; private set; }
        public string DescriptionPlaceholder { get; private set; }
        public string AdditionalInfoPlaceholder { get; private set; }
        public string AddButtonText { get; private set; }
        public string CancelButtonText { get; private set; }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string Description
        { 
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public string AdditionalInfo
        {
            get => _additionalInfo;
            set
            {
                _additionalInfo = value;
                OnPropertyChanged();
            }
        }
    }
}
