using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTestTaskAdvantica.ViewModels.Workers;

namespace WpfTestTaskAdvantica.Views.Workers
{
    /// <summary>
    /// Логика взаимодействия для CreateUpdateWorkerWindow.xaml
    /// </summary>
    public partial class CreateUpdateWorkerWindow : Window
    {
        public CreateUpdateWorkerWindow()
        {
            InitializeComponent();

            // инициализация контекста и события закрытия окна
            var vm = new CreateUpdateWorkerViewModel();
            vm.OnRequestClose += (s, e) => Close();
            DataContext = vm;
        }
    }
}
