using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using OpenBreed.Editor.VM;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OpenBreed.Editor.UI.Wpf.TemplateSelectors
{
    public class Selector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //if(container is not FrameworkElement frameworkElement)
            //{
            //    throw new InvalidOperationException($"Expected container of type '{nameof(FrameworkElement)}'");
            //}

            if (item is not EntryEditorVM entryEditor)
            {
                throw new InvalidOperationException($"Expected data context of type '{nameof(EntryEditorVM)}'");
            }

            var rectangleFactory = new FrameworkElementFactory(entryEditor.InnerCtrl.GetType());
            return new DataTemplate
            {
                VisualTree = rectangleFactory,
            };
        }
    }
}
