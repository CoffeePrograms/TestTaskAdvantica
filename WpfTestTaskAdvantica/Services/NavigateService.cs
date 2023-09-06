using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppRestaurant.ViewModel;

namespace WpfTestTaskAdvantica.Services
{
    /// <summary>
    /// Навигация между окнами через ViewModel
    /// </summary>
    static class NavigateService
    {
        /// <summary>
        /// Открытие диалогового окна
        /// </summary>
        /// <typeparam name="T">тип окна</typeparam>
        /// <typeparam name="D">тип ViewModel</typeparam>
        /// <param name="param">параметры для ViewModel</param>
        /// <returns>ViewModel для доступа к результатам работы окна</returns>
        public static D? ShowDialogWindow<T, D>(params object?[] param)
            where T : Window, new()
            where D : class, IViewModel
        {
            var window = new T();
            // Если контекст задан, то передаем в него параметры (например, данные соотрудника для последующего редактирования)
            if (window.DataContext is not null and D vm)
            {
                vm.SetParam(param);
            }
            window.ShowDialog();
            return window.DataContext as D;
        }
    }
}
