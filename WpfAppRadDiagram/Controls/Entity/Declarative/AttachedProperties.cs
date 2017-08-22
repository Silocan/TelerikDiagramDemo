using System.Windows;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace WpfAppRadDiagram.Controls.Entity
{
    public static class AttachedProperties
    {
        public static readonly DependencyProperty CustomConnectorsProperty =
            DependencyProperty.RegisterAttached("CustomConnectors", typeof(ConnectorCollection), typeof(AttachedProperties), new PropertyMetadata(null, OnCustomConnectorsChanged));

        public static ConnectorCollection GetCustomConnectors(DependencyObject obj)
        {
            return (ConnectorCollection)obj.GetValue(CustomConnectorsProperty);
        }

        public static void SetCustomConnectors(DependencyObject obj, ConnectorCollection value)
        {
            obj.SetValue(CustomConnectorsProperty, value);
        }

        private static void OnCustomConnectorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var shape = d as IShape;
            if (shape != null && e.NewValue != null)
            {
                shape.Connectors.Clear();
                foreach (RadDiagramConnector item in e.NewValue as ConnectorCollection)
                {
                    shape.Connectors.Add(new RadDiagramConnector() { Offset = item.Offset, Name = item.Name, Opacity = item.Opacity });
                }
            }
        }
    }
}
