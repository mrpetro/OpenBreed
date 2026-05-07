using OpenBreed.Editor.VM.Database.Entries;
using OpenBreed.Editor.VM.EntityClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenBreed.Editor.UI.Wpf.EntityClasses
{
    /// <summary>
    /// Interaction logic for EntityClassesTreeEditorCtrl.xaml
    /// </summary>
    public partial class EntityClassesTreeEditorCtrl : UserControl
    {
        public EntityClassesTreeEditorCtrl()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is EntityClassesTreeEditorVM vm)
            {
                vm.SelectedClass = e.NewValue as EntityClassNodeVM;
            }
        }

        private void TreeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            TreeView tree = sender as TreeView;
            TreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);

            if (item != null)
            {
                DragDrop.DoDragDrop(item, item.DataContext, DragDropEffects.Move);
            }
        }

        private TreeViewItem GetNearestContainer(UIElement element)
        {
            while (element != null && !(element is TreeViewItem))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
            }
            return element as TreeViewItem;
        }

        private void TreeView_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(EntityClassNodeVM)))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(EntityClassNodeVM)))
                return;

            var dragged = e.Data.GetData(typeof(EntityClassNodeVM)) as EntityClassNodeVM;

            TreeViewItem targetItem = GetNearestContainer(e.OriginalSource as UIElement);
            if (targetItem == null)
                return;

            var target = targetItem.DataContext as EntityClassNodeVM;

            var vm = DataContext as EntityClassesTreeEditorVM;
            vm?.MoveNodeCommand.Execute((dragged, target));
        }

        private bool RemoveFromParent(EntityClassNodeVM node, ObservableCollection<EntityClassNodeVM> collection)
        {
            if (collection.Contains(node))
            {
                collection.Remove(node);
                return true;
            }

            foreach (var child in collection)
            {
                if (RemoveFromParent(node, child.Children))
                    return true;
            }

            return false;
        }

        private bool IsDescendant(EntityClassNodeVM source, EntityClassNodeVM target)
        {
            if (source.Children.Contains(target))
                return true;

            foreach (var child in source.Children)
            {
                if (IsDescendant(child, target))
                    return true;
            }

            return false;
        }
    }
}
