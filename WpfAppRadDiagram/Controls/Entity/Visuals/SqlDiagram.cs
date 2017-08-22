using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace WpfAppRadDiagram.Controls.Entity
{
    public class SqlDiagram : RadDiagram
    {
        protected override IContainerShape GetShapeContainerForItemOverride(IContainerItem item)
        {
            return new TableShape();
        }

        protected override IShape GetShapeContainerForItemOverride(object item)
        {
            return new RowShape();
        }

        protected override bool IsItemItsOwnShapeContainerOverride(object item)
        {
            return item is RadDiagramShapeBase;
        }

        public override void Paste()
        {
            var selectedContainer = this.ContainerGenerator.ContainerFromItem(this.SelectedItem) as TableShape;
            if (selectedContainer != null)
                base.Paste();
        }
    }
}
