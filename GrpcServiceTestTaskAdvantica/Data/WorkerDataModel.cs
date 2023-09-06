using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServiceTestTaskAdvantica.Data
{
    [Table("Worker")]
    public class WorkerDataModel
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(255)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(255)]
        public string MiddleName { get; set; } = string.Empty;
        public long Birthday { get; set; }
        public int Sex { get; set; }
        public bool HaveChildren { get; set; }
    }
}
