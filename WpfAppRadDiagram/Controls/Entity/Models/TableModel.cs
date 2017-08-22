using System.Windows;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace WpfAppRadDiagram.Controls.Entity.Models
{
    public class TableModel : ContainerNodeViewModelBase<NodeViewModelBase>
    {
        private bool isCollapsed;

        public bool IsCollapsed
        {
            get
            {
                return this.isCollapsed;
            }
            set
            {
                if (this.isCollapsed != value)
                {
                    this.isCollapsed = value;
                    this.OnPropertyChanged("IsCollapsed");
                }
            }
        }

        public override bool AddItem(object item)
        {
            var viewModel = item as RowModel;
            if (viewModel != null)
            {
                viewModel.Position = new Point(this.Position.X, this.Position.Y + 90 + this.InternalItems.Count * 30);
                return base.AddItem(item);
            }

            return false;
        }

        public override bool RemoveItem(object item)
        {
            if (item is NodeViewModelBase)
                return base.RemoveItem(item);

            return false;
        }
    }
}
