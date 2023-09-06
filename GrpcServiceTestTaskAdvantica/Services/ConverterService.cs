using GrpcServiceTestTaskAdvantica.Data;
using LibProto;

namespace GrpcServiceTestTaskAdvantica.Services
{
    /// <summary>
    /// Конвертация типа сообщения в модель базы данных и наоборот
    /// </summary>
    public static class ConverterService
    {
        public static WorkerMessage ToMessage(WorkerDataModel item)
        {
            var worker = new WorkerMessage
            {
                Id = item.Id,
                LastName = item.LastName,
                FirstName = item.FirstName,
                MiddleName = item.MiddleName,
                Sex = (Sex)item.Sex,
                Birthday = item.Birthday,
                HaveChildren = item.HaveChildren,
            };
            return worker;
        }
        
        public static WorkerDataModel ToModel(WorkerMessage item)
        {
            var worker = new WorkerDataModel
            {
                Id = item.Id,
                LastName = item.LastName,
                FirstName = item.FirstName,
                MiddleName = item.MiddleName,
                Sex = (int)item.Sex,
                Birthday = item.Birthday,
                HaveChildren = item.HaveChildren,
            };
            return worker;
        }
    }
}
