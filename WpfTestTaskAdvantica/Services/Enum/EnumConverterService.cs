using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfTestTaskAdvantica.Models
{
    /// <summary>
    /// Конверататор значений для перечисления. Используется для отображения атрибута Display
    /// </summary>
    public class EnumConverterService : EnumConverter
    {
        public EnumConverterService(Type type)
            : base(type)
        {
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());
                    if (fi != null)
                    {
                        var dattributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        return dattributes.Select(d => d.Description)
                                   .FirstOrDefault() ?? string.Empty;
                    }
                }

                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}