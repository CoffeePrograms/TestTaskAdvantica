using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using LibProto;
using System.Diagnostics;
using System.Windows.Controls;
using System.Threading;
using WpfAppRestaurant.Service;
using WpfTestTaskAdvantica.Models;
using System.Windows.Media.Media3D;

namespace WpfTestTaskAdvantica.Api
{
    /// <summary>
    /// Работа с сервером
    /// </summary>
    internal static class WorkerApiService
    {
        /// <summary>
        /// Конвертация сообщения в модель данных
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static WorkerModel ToModel(WorkerMessage message)
        {
            return new WorkerModel
            {
                Id = message.Id,
                LastName = message.LastName,
                FirstName = message.FirstName,
                MiddleName = message.MiddleName,
                Sex = (Gender)message.Sex,
                Birthday = DateTime.FromBinary(message.Birthday),
                HaveChildren = message.HaveChildren,
            };
        }

        /// <summary>
        /// Конвертация модели данных в сообщение
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static WorkerMessage ToMessage(WorkerModel model)
        {
            return new WorkerMessage()
            {
                Id = model.Id,
                LastName = model.LastName ?? "_",
                FirstName = model.FirstName ?? "_",
                MiddleName = model.MiddleName ?? "_",
                Sex = (Sex)model.Sex,
                Birthday = model.Birthday.ToBinary(),
                HaveChildren = model.HaveChildren,
            };
        }

        /// <summary>
        /// Получение всех записей
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<WorkerModel>> GetAll()
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:7151");

                var client = new WorkerIntegration.WorkerIntegrationClient(channel);

                var serverData = client.GetWorkerStream(new EmptyMessage());

                var responseStream = serverData.ResponseStream;

                List<WorkerModel> workers = new();
                while (await responseStream.MoveNext(new CancellationToken()))
                {

                    var response = responseStream.Current.Worker;
                    workers.Add(ToModel(response));
                }

                return workers.OrderBy(l => l.Id);
            }
            catch (Exception exc)
            {
                Trace.TraceError(exc.Message);
                Trace.TraceError(exc.Source);
                MessageBoxService.ShowMessageOk("Ошибка при работе с сервером \n" + exc.Message, "Ошибка", System.Windows.MessageBoxImage.Error);
                return new List<WorkerModel>();
            }
        }

        /// <summary>
        /// Изменение одной записи
        /// </summary>
        /// <param name="model">запись</param>
        /// <param name="action">тип действия, которое надо произвести над записью</param>
        /// <returns></returns>
        public static async Task CreateUpdateDelete(WorkerModel model, LibProto.Action action)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:7151");

                var client = new WorkerIntegration.WorkerIntegrationClient(channel);

                var message = new WorkerAction { Worker = ToMessage(model), ActionType = action };

                var serverData = await client.ActionWorkerAsync(message);

            }
            catch (Exception exc)
            {
                Trace.TraceError(exc.Message);
                Trace.TraceError(exc.Source);
                MessageBoxService.ShowMessageOk("Ошибка при работе с сервером \n" + exc.Message, "Ошибка", System.Windows.MessageBoxImage.Error);
            }

        }
    }
}
