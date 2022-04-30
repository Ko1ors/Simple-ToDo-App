using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ToDoApp.Models;

namespace ToDoApp.Converters
{
    [ValueConversion(typeof(bool), typeof(TaskStatusType))]
    public class BoolToTaskStatusTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? isDone = (bool?)value;
            if (!isDone.HasValue)
                return TaskStatusType.All;
            if (isDone.Value)
                return TaskStatusType.Completed;
            return TaskStatusType.Current;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var statusType = (TaskStatusType)value;
            if (statusType == TaskStatusType.Completed)
                return true;
            if (statusType == TaskStatusType.Current)
                return false;
            return null;
        }
    }
}
