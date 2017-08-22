using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Extensions;
using Telerik.Windows.Diagrams.Core;

namespace WpfAppRadDiagram.Controls.Entity
{

    public class AdditionalContent : Control
    {
        public static readonly DependencyProperty ContextItemProperty =
            DependencyProperty.Register("ContextItem", typeof(object), typeof(AdditionalContent), new PropertyMetadata(null, OnContextItemPropertyChanged));

        public static readonly DependencyProperty DiagramProperty =
            DependencyProperty.Register("Diagram", typeof(RadDiagram), typeof(AdditionalContent), new PropertyMetadata(null));

        protected SettingsPane settingsPane;
        protected FrameworkElement addRemove;

        public RadDiagram Diagram
        {
            get { return (RadDiagram)GetValue(DiagramProperty); }
            set { SetValue(DiagramProperty, value); }
        }

        public object ContextItem
        {
            get { return (object)GetValue(ContextItemProperty); }
            set { SetValue(ContextItemProperty, value); }
        }

        static AdditionalContent()
        {
            /*
            CommandManager.RegisterClassCommandBinding(typeof(AdditionalContent), new CommandBinding(SwimlaneCommands.AddCommand, OnAddCommand));
            CommandManager.RegisterClassCommandBinding(typeof(AdditionalContent), new CommandBinding(SwimlaneCommands.RemoveCommand, OnRemoveCommand, OnCanExecuteRemove));
            */
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.settingsPane = this.GetTemplateChild("settingsPane") as SettingsPane;
            this.addRemove = this.GetTemplateChild("addRemoveButtons") as FrameworkElement;

            if (this.ContextItem != null)
                this.OnContextItemChanged(this.ContextItem, null);
        }

        protected virtual void OnRemove()
        {
        }

        protected virtual void OnAdd()
        {
        }

        protected virtual void OnContextItemChanged(object newValue, object oldValue)
        {
        }

        protected virtual void OnCanRemove(CanExecuteRoutedEventArgs e)
        {
        }

        private static void OnCanExecuteRemove(object sender, CanExecuteRoutedEventArgs e)
        {
            var addCont = sender as AdditionalContent;
            if (addCont != null && addCont.Diagram != null)
            {
                addCont.OnCanRemove(e);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private static void OnRemoveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var addCont = sender as AdditionalContent;
            if (addCont != null && addCont.Diagram != null)
            {
                addCont.OnRemove();
            }
        }

        private static void OnAddCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var addCont = sender as AdditionalContent;
            if (addCont != null && addCont.Diagram != null)
            {
                addCont.OnAdd();
            }
        }

        private static void OnContextItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var additionalContent = d as AdditionalContent;
            if (additionalContent != null)
            {
                additionalContent.OnContextItemChanged(e.NewValue, e.OldValue);
            }
        }

        private CompositeCommand CreateRemoveShapeCommand(RadDiagramShapeBase shape)
        {
            if (shape == null) return null;
            var compositeRemoveShapeCommand = new CompositeCommand(CommandNames.RemoveShapes);
            var removeCommand = new UndoableDelegateCommand(CommandNames.RemoveShape, s => this.Diagram.RemoveShape(shape), s => this.Diagram.AddShape(shape));

            var parentContainer = shape.ParentContainer;
            if (parentContainer != null)
            {
                var execute = new Action<object>((o) => parentContainer.RemoveItem(shape));
                var undo = new Action<object>((o) =>
                {
                    if (!parentContainer.Items.Contains(shape))
                        parentContainer.AddItem(shape);
                });
                compositeRemoveShapeCommand.AddCommand(new UndoableDelegateCommand(CommandNames.RemoveItemFromContainer, execute, undo));
            }

            foreach (var changeSourceCommand in shape.OutgoingLinks.Union(shape.IncomingLinks).ToList().Select(connection => this.CreateRemoveConnectionCommand(connection)))
            {
                compositeRemoveShapeCommand.AddCommand(changeSourceCommand);
            }

            compositeRemoveShapeCommand.AddCommand(removeCommand);

            var container = shape as RadDiagramContainerShape;
            if (container != null)
            {
                for (int i = container.Items.Count - 1; i >= 0; i--)
                {
                    var shapeToRemove = container.Items[i] as RadDiagramShapeBase;
                    if (shapeToRemove != null)
                        compositeRemoveShapeCommand.AddCommand(this.CreateRemoveShapeCommand(shapeToRemove));
                    else
                    {
                        var connection = container.Items[i] as IConnection;
                        if (connection != null && connection.Source == null && connection.Target == null)
                            compositeRemoveShapeCommand.AddCommand(this.CreateRemoveConnectionCommand(container.Items[i] as IConnection));
                    }
                }
            }

            return compositeRemoveShapeCommand;
        }

        private UndoableDelegateCommand CreateRemoveConnectionCommand(IConnection connection)
        {
            var compositeRemoveConnectionCommand = new CompositeCommand(CommandNames.RemoveConnections);

            UndoableDelegateCommand removeCommand = new UndoableDelegateCommand(CommandNames.RemoveConnection, s => this.Diagram.RemoveConnection(connection), s => this.Diagram.AddConnection(connection));

            compositeRemoveConnectionCommand.AddCommand(removeCommand);

            return compositeRemoveConnectionCommand;
        }
    }
}
