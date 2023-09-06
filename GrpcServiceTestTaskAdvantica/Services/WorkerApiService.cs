using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceTestTaskAdvantica;
using GrpcServiceTestTaskAdvantica.Data;
using LibProto;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;
using System.Reflection;

namespace GrpcServiceTestTaskAdvantica.Services
{
    /// <summary>
    /// ������ � ��������
    /// </summary>
    public class WorkerApiService : WorkerIntegration.WorkerIntegrationBase
    {
        /// <summary>
        /// �������� ���� ������
        /// </summary>
        readonly DataContext db;
        
        public WorkerApiService(DataContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// ��������� ������ ����������� ����� �����
        /// </summary>
        /// <param name="request">����� ���������</param>
        /// <param name="responseStream">����� ��� ������ ������ � �����������</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GetWorkerStream(EmptyMessage request,
        IServerStreamWriter<WorkerAction> responseStream,
        ServerCallContext context)
        {
            var workers = db.Workers.Select(item => ConverterService.ToMessage(item)).ToList();

            foreach (var message in workers)
            {
                await responseStream.WriteAsync(new WorkerAction { Worker = message, ActionType = 0 });
            }
        }

        /// <summary>
        /// �������� ������ ���������� ������ ��� �������� ���������.
        /// ��� �������� ����������, ��� ������ � �������.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<EmptyMessage> ActionWorker(WorkerAction request, ServerCallContext context)
        {
            switch (request.ActionType)
            {
                case LibProto.Action.Create:
                    return await CreateWorker(request.Worker);
                case LibProto.Action.Update:
                    return await UpdateWorker(request.Worker);
                case LibProto.Action.Delete:
                    return await DeleteWorker(request.Worker);
                default:
                    break;
            }

            return new EmptyMessage();
        }

        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<EmptyMessage> CreateWorker(WorkerMessage item)
        {
            var worker = ConverterService.ToModel(item);
            await db.Workers.AddAsync(worker);
            await db.SaveChangesAsync();
            return new EmptyMessage();
        }

        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        private async Task<EmptyMessage> UpdateWorker(WorkerMessage request)
        {
            var item = ConverterService.ToModel(request);
            var worker = await db.Workers.FindAsync(item.Id);
            if (worker == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Worker not found"));
            }

            foreach (var property in worker.GetType().GetProperties())
            {
                property.SetValue(worker, property.GetValue(item, null));
            }
            await db.SaveChangesAsync();
            return new EmptyMessage();
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        private async Task<EmptyMessage> DeleteWorker(WorkerMessage item)
        {
            var worker = await db.Workers.FindAsync(item.Id);
            if (worker == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Worker not found"));
            }
            db.Workers.Remove(worker);
            await db.SaveChangesAsync();
            return new EmptyMessage();
        }
    }
}