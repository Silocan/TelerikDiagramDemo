using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using WpfAppRadDiagram.Controls.Entity.Models;

namespace WpfAppRadDiagram.Controls.Entity
{
    public class TableShape : RadDiagramContainerShape
    {
        protected override bool IsDropPossible { get { return false; } }
        
        /*
        protected override System.Windows.Rect CalculateContentBounds(System.Windows.Rect newShapeBounds)
        {
            return this.ContentBounds;
        }
        */
        
        protected override void OnItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsCollectionChanged(sender, e);
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var model = e.OldItems[0] as RowModel;
                if (model == null) return;

                foreach (var item in this.Items.OfType<RowModel>())
                {
                    if (item.Position.Y >= model.Position.Y)
                    {
                        item.Position = new Point(item.Position.X, item.Position.Y - 30);
                    }
                }
                this.Height -= 30;
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var model = e.NewItems[0] as RowModel;
                if (model == null) return;

                if (this.Bounds.Bottom - 25 < model.Position.Y && (this.Diagram as RadDiagram).UndoRedoService.IsActive)
                    this.ContentBounds = this.CalculateContentBounds(this.Bounds);

                this.RefreshConnections();
            }
        }

        private void RefreshConnections()
        {
            foreach (var connection in this.Diagram.Connections)
            {
                connection.Update();
            }
        }
    }
}
