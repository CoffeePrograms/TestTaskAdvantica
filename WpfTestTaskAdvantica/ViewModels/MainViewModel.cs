using Grpc.Net.Client;
using LibProto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppRestaurant.Service;
using WpfTestTaskAdvantica.Api;
using WpfTestTaskAdvantica.Commands;
using WpfTestTaskAdvantica.Models;
using WpfTestTaskAdvantica.Services;
using WpfTestTaskAdvantica.ViewModels.Workers;
using WpfTestTaskAdvantica.Views.Workers;

namespace WpfTestTaskAdvantica.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Список сотрудников для хранения и отображения 
        /// </summary>
        private ObservableCollection<WorkerModel> workers;
        
        /// <summary>
        /// Выбранный в таблице сотрудник (для изменения или удаления)
        /// </summary>
        private WorkerModel selectedWorker;

        // команды для кнопок
        // getAllWorkersCommand используется для обновления таблицы (например, при перезапуске сервера)
        private readonly ICommand getAllWorkersCommand;
        private readonly ICommand createWorkerCommand;
        private readonly ICommand updateWorkerCommand;
        private readonly ICommand deleteWorkerCommand;

        #endregion

        #region Properties
        public ObservableCollection<WorkerModel> Workers
        {
            get { return workers; }
            set
            {
                workers = value;
                OnPropertyChanged("Workers");
            }
        }

        public WorkerModel SelectedWorker
        {
            get { return selectedWorker; }
            set
            {
                selectedWorker = value;
                OnPropertyChanged("SelectedWorker");
            }
        }
        #endregion

        #region Commands
        public ICommand GetAllWorkersCommand
        {
            get { return getAllWorkersCommand; }
        }

        public ICommand CreateWorkerCommand
        {
            get { return createWorkerCommand; }
        }

        public ICommand UpdateWorkerCommand
        {
            get { return updateWorkerCommand; }
        }

        public ICommand DeleteWorkerCommand
        {
            get { return deleteWorkerCommand; }
        }
        #endregion

        public MainViewModel()
        {
            getAllWorkersCommand = new RelayCommand(GetAllWokers);
            GetAllWokers(null);
            
            createWorkerCommand = new RelayCommand(CreateWorker);
            updateWorkerCommand = new RelayCommand(UpdateWorker);
            deleteWorkerCommand = new RelayCommand(DeleteWorker);
        }

        #region Methods

        /// <summary>
        /// Получение записей с сервера
        /// </summary>
        /// <param name="parameter"></param>
        private async void GetAllWokers(object? parameter)
        {
            Workers = new ObservableCollection<WorkerModel>(await WorkerApiService.GetAll());
        }

        /// <summary>
        /// Открытие окна добавления сотрудника
        /// </summary>
        /// <param name="parameter">пустой</param>
        private void CreateWorker(object? parameter)
        {
            try
            {
                // открытие дочернего окна через сервис
                var vm = NavigateService.ShowDialogWindow<CreateUpdateWorkerWindow, CreateUpdateWorkerViewModel>();
                
                // если пользователь сохранил изменения, то обновление списка сотрудников
                if (vm != null && vm.IsConfirm)
                {
                    GetAllWokers(null);
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError(exc.Message);
                MessageBoxService.ShowMessageOk("Ошибка при создании объекта", "Ошибка", MessageBoxImage.Warning);

            }
        }

        /// <summary>
        /// Открытие окна обновления сотрудника
        /// </summary>
        /// <param name="parameter">сотрудник</param>
        private void UpdateWorker(object? parameter)
        {
            if (parameter == null)
            {
                MessageBoxService.ShowMessageOk("Выберите строку в таблице", "Ошибка", MessageBoxImage.Warning);
                return;
            }

            try
            {
                // открытие дочернего окна через сервис. в качестве parameter передается выбранная в таблице запись
                var vm = NavigateService.ShowDialogWindow<CreateUpdateWorkerWindow, CreateUpdateWorkerViewModel>(parameter);
                
                // если пользователь сохранил изменения, то обновление списка сотрудников
                if (vm != null && vm.IsConfirm)
                {
                    GetAllWokers(null);
                }
            }
            catch (Exception exc)
            {
                MessageBoxService.ShowMessageOk("Ошибка при обновлении записи \n" + exc.Message, "Ошибка", MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="parameter">сотрудник</param>
        private async void DeleteWorker(object? parameter)
        {
            // если параметр нулл, пользователь не выбрал запись
            if (parameter == null)
                MessageBoxService.ShowMessageOk("Выберите строку в таблице", "Ошибка", MessageBoxImage.Information);
            try
            {
                // если параметр не WorkerModel, значит ошибка в передаче параметра и требуется соответсующее сообщение об ошибке
                if (parameter is not WorkerModel worker)
                    throw new ArgumentException("Параметр должен быть типа Сотрудник");
                
                // отправка серверу запроса на удаление
                await WorkerApiService.CreateUpdateDelete(worker, LibProto.Action.Delete);
                
                // обновление списка сотрудников
                GetAllWokers(null);
            }
            catch (Exception exc)
            {
                MessageBoxService.ShowMessageOk("Ошибка при удалении записи \n" + exc.Message, "Ошибка", MessageBoxImage.Warning);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
        #endregion

        #endregion
    }
}