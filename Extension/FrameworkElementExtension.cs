using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public static class FrameworkElementExtension
    {
        public static T FindName<T>(this FrameworkElement element, string elementName)
        {
            return (T)element.FindName(elementName);
        }
    }
}
