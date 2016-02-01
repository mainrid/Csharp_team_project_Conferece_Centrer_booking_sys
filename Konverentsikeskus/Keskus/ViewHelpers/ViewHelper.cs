using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Keskus.ViewHelpers
{
     public class ViewHelper
    {
        public ViewHelper()
        {
        }

        /// <remarks>
        /// Method replacing "select a date" placeholder on DatePickers. Solution from https://social.msdn.microsoft.com/Forums/vstudio/en-US/9eec87e0-4d12-430d-83fd-ce13dd96776b/datepicker-hide-select-date-placeholder-or-change-it?forum=wpf
        /// </remarks>
        /// <param name="sender"></param>
        public void clearDatePickerPlaceholder(Object sender)
        {
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                DatePickerTextBox datePickerTextBox = FindVisualChild<DatePickerTextBox>(datePicker);
                if (datePickerTextBox != null)
                {

                    ContentControl watermark = datePickerTextBox.Template.FindName("PART_Watermark", datePickerTextBox) as ContentControl;
                    if (watermark != null)
                    {
                        watermark.Content = string.Empty;

                    }
                }
            }
        }

        /// <remarks>
        /// Method extracts DatePickerTextBox of DatePicker control
        /// Part of clearDatePickerPlaceholder() method.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="depencencyObject"></param>
        /// <returns></returns>
        private T FindVisualChild<T>(DependencyObject depencencyObject) where T : DependencyObject
        {
            if (depencencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depencencyObject); ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depencencyObject, i);
                    T result = (child as T) ?? FindVisualChild<T>(child);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        /// <remarks>
        /// checking user input (keyborad) against non-numeric values
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regular expression that matches allowed text
            return !regex.IsMatch(text);
        }

    }
}
