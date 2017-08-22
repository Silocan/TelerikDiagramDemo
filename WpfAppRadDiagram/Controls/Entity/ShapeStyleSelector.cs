using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfAppRadDiagram.Controls.Entity.Models;

namespace WpfAppRadDiagram.Controls.Entity
{

    public class ShapeStyleSelector : StyleSelector
    {
        public Style RowStyle { get; set; }
        public Style TableStyle { get; set; }

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            if (item is RowModel)
                return this.RowStyle;

            return this.TableStyle;
        }
    }
}
