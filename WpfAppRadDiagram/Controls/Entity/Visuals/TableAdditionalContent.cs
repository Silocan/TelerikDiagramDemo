using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;
using WpfAppRadDiagram.Controls.Entity.Models;

namespace WpfAppRadDiagram.Controls.Entity
{
    public class TableAdditionnalContent : AdditionalContent
    {
        protected override void OnContextItemChanged(object newValue, object oldValue)
        {
            if (newValue is TableModel)
            {
                this.Visibility = System.Windows.Visibility.Visible;
                if (this.addRemove != null)
                    this.addRemove.Visibility = System.Windows.Visibility.Visible;
                if (this.settingsPane != null)
                {
                    this.settingsPane.Opacity = 0;
                    this.settingsPane.IsHitTestVisible = false;
                }
            }
            else
                this.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnAdd()
        {
            var model = this.ContextItem as TableModel;
            if (model != null)
            {
                var itemToAdd = new RowModel() { ColumnName = "NewRow", DataType = DataType.String };
                var addCommand = new UndoableDelegateCommand("Add new row", new Action<object>((o) => this.AddNewRow(model, itemToAdd)), new Action<object>((o) => this.RemoveRow(model, itemToAdd)));
                this.Diagram.UndoRedoService.ExecuteCommand(addCommand);
            }
        }

        protected override void OnCanRemove(CanExecuteRoutedEventArgs e)
        {
            var model = this.ContextItem as TableModel;
            if (model != null && model.InternalItems.Count > 0)
            {
                e.CanExecute = true;
            }
        }

        protected override void OnRemove()
        {
            var model = this.ContextItem as TableModel;
            if (model != null && model.InternalItems.Count > 0)
            {
                var itemToRemove = model.InternalItems.LastOrDefault();
                var removeCommand = new UndoableDelegateCommand("Add new row", new Action<object>((o) => this.RemoveRow(model, itemToRemove)), new Action<object>((o) => this.AddNewRow(model, itemToRemove)));
                this.Diagram.UndoRedoService.ExecuteCommand(removeCommand);
            }
        }

        private void AddNewRow(TableModel model, NodeViewModelBase itemToRemove)
        {
            if (model.IsCollapsed)
                model.IsCollapsed = false;
            if (itemToRemove == null)
                model.AddItem(new RowModel() { ColumnName = "NewRow", DataType = DataType.String });
            else
                model.AddItem(itemToRemove);
        }

        private void RemoveRow(TableModel model, NodeViewModelBase itemToRemove)
        {
            var dc = this.DataContext as TablesGraphSource;
            if (dc != null && model != null && itemToRemove != null)
            {
                if (model.IsCollapsed)
                    model.IsCollapsed = false;
                dc.RemoveItem(itemToRemove);
            }

            CommandManager.InvalidateRequerySuggested();
        }
    }
}
