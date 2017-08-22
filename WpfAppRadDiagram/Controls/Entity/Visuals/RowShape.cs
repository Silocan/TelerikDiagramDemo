using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace WpfAppRadDiagram.Controls.Entity
{
    public class RowShape : RadDiagramShape
    {
        private readonly string connectionToolName = "Connection Tool";
        private bool isButtonDown;
        private Point lastDownPosition;

        public RowShape()
            : base()
        {
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.isButtonDown = false;
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.lastDownPosition = e.GetPosition(this);
            this.isButtonDown = true;
        }
        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            this.isButtonDown = false;
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.isButtonDown && !this.IsInEditMode &&
                !this.connectionToolName.Equals(this.Diagram.ServiceLocator.GetService<IToolService>().ActiveTool.Name))
            {
                if (!this.lastDownPosition.AroundPoint(e.GetPosition(this), 2))
                {
                    var diagram = this.Diagram as RadDiagram;
                    if (diagram != null)
                        diagram.SelectedItem = diagram.ContainerGenerator.ItemFromContainer(this);
                    this.isButtonDown = false;
                    var toolService = this.Diagram.ServiceLocator.GetService<IToolService>();
                    toolService.ActivateTool(this.connectionToolName);
                    toolService.MouseDown(new PointerArgs());
                }
            }
            else
            {
                this.lastDownPosition = new Point(-100, -100);
                this.isButtonDown = false;
            }
        }
    }
}
