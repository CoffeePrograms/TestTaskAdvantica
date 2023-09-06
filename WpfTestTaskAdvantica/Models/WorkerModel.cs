using Google.Protobuf.WellKnownTypes;
using LibProto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfTestTaskAdvantica.Models
{ 
    public class WorkerModel
    {
        public int Id { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public DateTime Birthday { get; set; }

        public Gender Sex { get; set; }

        public bool HaveChildren { get; set; }
    }


    [TypeConverter(typeof(EnumConverterService))]
    public enum Gender
    {
        [Description("—")]
        DefaultSex = 0,
        [Description("Мужчина")] 
        Male = 1,
        [Description("Женщина")]
        Female = 2
    }
}
