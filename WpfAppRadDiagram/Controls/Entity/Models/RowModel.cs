using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace WpfAppRadDiagram.Controls.Entity.Models
{
    public class RowModel : NodeViewModelBase
    {
        private string columnName;
        private DataType dataType;

        public DataType DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                if (this.dataType != value)
                {
                    this.dataType = value;
                    this.OnPropertyChanged("DataType");
                }
            }
        }

        public string ColumnName
        {
            get
            {
                return this.columnName;
            }
            set
            {
                if (this.columnName != value)
                {
                    this.columnName = value;
                    this.OnPropertyChanged("ColumnName");
                }
            }
        }
    }
}
