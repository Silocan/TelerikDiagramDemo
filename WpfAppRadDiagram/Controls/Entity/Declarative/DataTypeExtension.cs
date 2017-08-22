using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WpfAppRadDiagram.Controls.Entity
{

    /// <summary>
    /// DataType MarkupExtension.
    /// </summary>
    public class DataTypeExtension : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeExtension"/> class.
        /// </summary>
        public DataTypeExtension()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(typeof(DataType));
        }
    }
}
