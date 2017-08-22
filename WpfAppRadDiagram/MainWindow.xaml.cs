using System;
using System.Collections.Generic;
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
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;
using WpfAppRadDiagram.Controls.Entity;
using WpfAppRadDiagram.Controls.Entity.Models;

namespace WpfAppRadDiagram
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected TablesGraphSource _graphSource;

        public MainWindow()
        {
            InitializeComponent();

            _graphSource= new TablesGraphSource();
            tlrDiagram.GraphSource = _graphSource;
        }

        private void OnItemsChanging(object sender, DiagramItemsChangingEventArgs e)
        {
            //if (this.isClear) return;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var rowModel = e.OldItems.ElementAt(0) as RowModel;
                var command = new CompositeCommand("Remove Connections");
                if (rowModel != null)
                {
                    this.RemoveRowModel(rowModel, command);
                    if (command.Commands.Count() > 0)
                        this.tlrDiagram.UndoRedoService.ExecuteCommand(command);
                }
                else
                {
                    var tableModel = e.OldItems.ElementAt(0) as TableModel;
                    if (tableModel != null)
                    {
                        foreach (var item in tableModel.InternalItems)
                        {
                            this.RemoveRowModel(item as RowModel, command);
                        }
                        if (command.Commands.Count() > 0)
                            this.tlrDiagram.UndoRedoService.ExecuteCommand(command);

                        tableModel.InternalItems.Clear();
                    }
                }
            }
        }

        private void RemoveRowModel(RowModel rowModel, CompositeCommand command)
        {
            var container = this.tlrDiagram.ContainerGenerator.ContainerFromItem(rowModel) as IShape;
            if (container == null) return;

            foreach (var connection in this.tlrDiagram.GetConnectionsForShape(container).ToList())
            {
                var link = this.tlrDiagram.ContainerGenerator.ItemFromContainer(connection) as LinkViewModelBase<NodeViewModelBase>;
                command.AddCommand(new UndoableDelegateCommand(
                    "Remove link",
                    new Action<object>((o) => (this.tlrDiagram.GraphSource as TablesGraphSource).RemoveLink(link)),
                    new Action<object>((o) => (this.tlrDiagram.GraphSource as TablesGraphSource).AddLink(link))));
            }
        }

        private void tlrDiagram_PreviewAdditionalContentActivated(object sender, AdditionalContentActivatedEventArgs e)
        {

        }

        private void btnAddTable_Click(object sender, RoutedEventArgs e)
        {

            (tlrDiagram.GraphSource as TablesGraphSource).AddNode(new TableModel()
            {
                Position = new Point(150, 150),
                Content = "tableName"
            });
        }

        private void btnAddRow_Click(object sender, RoutedEventArgs e)
        {

            if (tlrDiagram.SelectedItem is TableModel)
            {
                var model = tlrDiagram.SelectedItem as TableModel;
                if (model.IsCollapsed)
                    model.IsCollapsed = false;
                model.AddItem(new RowModel() { ColumnName = "NewRow", DataType = DataType.String });

            }
        }
    }
}
