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

namespace MAUtour.ViewModels.Dialogs
{
    internal class DialogViewModel : BaseViewModel
    {
        private string _name;
        private string _description;
        private string _additionalInfo;
        private IUnitOfWork _unitOfWork;
        public DialogViewModel(Popup popup, IUnitOfWork unitOfWork, bool isPin = false)
        {
            InizializeDialog(isPin, popup);
            _unitOfWork = unitOfWork;
            _unitOfWork.pinRepository.GetAllAsync();
        }

        private void InizializeDialog(bool isPin, Popup popup)
        {
            NameLabel = "Наименование";
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
                    return;
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
