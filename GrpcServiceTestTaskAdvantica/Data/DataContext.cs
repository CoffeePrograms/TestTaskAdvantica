using GrpcServiceTestTaskAdvantica.Data;
using LibProto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
//using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceTestTaskAdvantica.Data
{
    /*  Памятка по работе с CodeFirst
     *  
     *  Создание / обновление БД
     *  удалить папку миграций и бд на сервере
     *  в консоли ввести
     *  cd GrpcServiceTestTaskAdvantica
     *  dotnet ef migrations add Init
     *  Для обновления БД: 
     *      dotnet ef database update
     *  Для выгрузки запросов: 
     *      dotnet ef migrations script
     */

    public class DataContext : DbContext
    {
        public DbSet<WorkerDataModel> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User id=postgres;password=123;database=advantica");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkerDataModel>()
                .HasData(
                    new WorkerDataModel
                    {
                        Id = 1,
                        LastName = "Ивано",
                        FirstName = "Иван",
                        MiddleName = "Иванович",
                        Birthday = new DateTime(1990, 1, 4).ToBinary(),
                        Sex = 1,
                        HaveChildren = false
                    },
                    new WorkerDataModel
                    {
                        Id = 2,
                        LastName = "Петров",
                        FirstName = "Петр",
                        MiddleName = "Петрович",
                        Birthday = new DateTime(1988, 5, 7).ToBinary(),
                        Sex = 1,
                        HaveChildren = true
                    },
                    new WorkerDataModel
                    {
                        Id = 3,
                        LastName = "Николаева",
                        FirstName = "Ника",
                        MiddleName = "Николаевна",
                        Birthday = new DateTime(1970, 3, 8).ToBinary(),
                        Sex = 2,
                        HaveChildren = false
                    }
                );
        }
    }
}
