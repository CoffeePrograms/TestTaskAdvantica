using LibProto;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppRestaurant.ViewModel;
using WpfTestTaskAdvantica.Api;
using WpfTestTaskAdvantica.Commands;
using WpfTestTaskAdvantica.Models;

namespace WpfTestTaskAdvantica.ViewModels.Workers
{
    public class CreateUpdateWorkerViewModel : IViewModel, INotifyPropertyChanged
    {
        #region Fields
        // Поля для вьюшки
        private int id;
        private string lastName, firstName, middleName;
        private DateTime birthday;
        private bool haveChildren;

        // Выбранный пол
        private Gender selectedGender;

        private readonly ICommand confirmCommand, cancelCommand;

        private LibProto.Action workerAction;

        #endregion

        #region Properties
        /// <summary>
        /// Флаг принятых изменений
        /// </summary>
        public bool IsConfirm { get; private set; }

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }

        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public bool HaveChildren
        {
            get { return haveChildren; }
            set
            {
                haveChildren = value;
                OnPropertyChanged("HaveChildren");
            }
        }

        public Gender SelectedGender
        {
            get
            {
                return selectedGender;
            }
            set
            {
                selectedGender = value;
                OnPropertyChanged("SelectedGender");
            }
        }

        #endregion


        #region Commands
        public ICommand ConfirmCommand
        {
            get { return confirmCommand; }
        }
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
        }

        #endregion


        #region Events

        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        public event EventHandler OnRequestClose;

        #endregion


        /* Через конструктор не передается сотрудник потому,
         * что вьюмодел инициализируется в окне вместе с действием закрытия окна, и,
         * при перезаписи вьюмодел, событие закрытия окна очистится.
         * 
         * Изначально вьюмодел в состоянии добавления записи.
         * В состояние редактирования вьюмодел переходит при передачи данных сотрудника через SetParam.
         */
        public CreateUpdateWorkerViewModel()
        {
            IsConfirm = false;
            // по умолчанию в состоянии Create, в состояние Update может перейти после вызова SetParam.
            workerAction = LibProto.Action.Create;

            confirmCommand = new RelayCommand(Confirm);
            cancelCommand = new RelayCommand(Cancel);

            // установление даты рождения по умолчанию
            Birthday = DateTime.Now.AddYears(-18);
        }

        #region Methods

        /// <summary>
        /// Передача данных о сотруднике после создания окна. 
        /// Вызывается из шаблонного метода класса по навигации между окнами, поэтому тип параметров object
        /// </summary>
        /// <param name="param">список параметров</param>
        public void SetParam(params object?[] param)
        {
            // Если параметра нет или он не требуемого типа, то ничего не меняем
            if ((param.Length == 0) || (param[0] is not WorkerModel worker))
            {
                return;
            }

            // Иначе устанаваливаем состояние в апдейт и фиксируем принятые данные

            workerAction = LibProto.Action.Update;

            Id = worker.Id;
            LastName = worker.LastName;
            FirstName = worker.FirstName;
            MiddleName = worker.MiddleName;
            SelectedGender = worker.Sex;
            Birthday = worker.Birthday;
            HaveChildren = worker.HaveChildren;
        }

        private async void Confirm(object? parameter)
        {
            // фиксируем, что пользователь решил внести изменения
            IsConfirm = true;

            // создаем объект по данным полей
            WorkerModel worker = new()
            {
                Id = Id,
                LastName = LastName ?? "_",
                FirstName = FirstName ?? "_",
                MiddleName = MiddleName ?? "_",
                Sex = SelectedGender,
                Birthday = Birthday,
                HaveChildren = HaveChildren,
            };
            // передаем на сервер данные по сотруднику и тип изменений (Create / Update)
            await WorkerApiService.CreateUpdateDelete(worker, workerAction);

            // вызываем событие закрытия окна
            OnRequestClose(this, new EventArgs());
        }

        private void Cancel(object? parameter)
        {
            // фиксируем, что пользователь решил внести изменения
            IsConfirm = false;

            // вызываем событие закрытия окна
            OnRequestClose(this, new EventArgs());
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

    }
}
