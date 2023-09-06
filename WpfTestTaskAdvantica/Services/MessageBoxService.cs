using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppRestaurant.Service
{
    /// <summary>
    /// Запуск окна с сообщением из ViewModel
    /// </summary>
    static class MessageBoxService
    {
        public static MessageBoxResult ShowMessageOk(string text, string caption, MessageBoxImage messageType)
        {
            var message = MessageBox.Show(text, caption, MessageBoxButton.OK, messageType);
            return message;
        }
    }
}
