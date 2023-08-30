using CommunityToolkit.Maui.Views;

using Mapsui;

using MAUtour.Local.Models;
using MAUtour.Local.UnitOfWork.Interface;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUtour.ViewModels.Dialogs
{
    internal class RouteDialogViewModel : BaseViewModel
    {
        private IUnitOfWork _unitOfWork;
        private string _name;
        private string _description;
        private string _additionalInfo;

        public ObservableCollection<RouteTypes> RouteTypes { get; set; }
        public RouteTypes SelectedRouteTypes { get; set; }

        public RouteDialogViewModel(Popup popup, IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
            InitializeDialog(popup);
        }

        private void InitializeDialog(Popup popup)
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
            Title = "Добавление нового маршрута";
            RouteTypes = new ObservableCollection<RouteTypes>(_unitOfWork.routeTypesRepository.GetAllAsync().Result);
            SelectedRouteTypes = new();

            Add = new Command(async (obj) =>
            {
                var test = new Routes
                {
                    RouteTypeId = SelectedRouteTypes.Id,
                    Name = Name,
                    Description = Description
                };
                await _unitOfWork.routesRepository.AddAsync(test);
                await _unitOfWork.CommitAsync();
                popup.Close(true);
            });
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
