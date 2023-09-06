using System;

namespace WpfAppRestaurant.ViewModel
{
    /// <summary>
    /// Интерфейс для моделей представлений
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Событие для закрытия окна. Должно определятся в констуркторе View. 
        /// </summary>
        event EventHandler OnRequestClose;

        /// <summary>
        /// Метод передачи данных между ViewModel разных окон.
        /// </summary>
        /// <param name="param"></param>
        void SetParam(params object?[] param);
    }
}