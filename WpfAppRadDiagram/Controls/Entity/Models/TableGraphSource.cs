using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;
using WpfAppRadDiagram.Controls.Entity.Models;

namespace WpfAppRadDiagram.Controls.Entity
{

    public class TablesGraphSource : SerializableGraphSourceBase<NodeViewModelBase, LinkViewModelBase<NodeViewModelBase>>
    {
        public TablesGraphSource()
        {
        }

        public override string GetNodeUniqueId(NodeViewModelBase node)
        {
            if (node != null)
                return node.GetHashCode().ToString();

            return string.Empty;
        }

        public override void SerializeNode(NodeViewModelBase node, SerializationInfo info)
        {
            base.SerializeNode(node, info);

            var row = node as RowModel;
            if (row != null)
            {
                info["ColumnName"] = row.ColumnName.ToString();
                info["DataType"] = row.DataType.ToString();
            }
            else
            {
                var table = node as TableModel;
                if (table != null)
                {
                    info["Content"] = table.Content.ToString();
                    info["IsCollapsed"] = null;
                    info["MyIsCollapsed"] = table.IsCollapsed.ToString();
                }
            }

            info["Position"] = string.Empty;
            info["MyPosition"] = node.Position.ToInvariant();
        }

        public override NodeViewModelBase DeserializeNode(IShape shape, SerializationInfo info)
        {
            NodeViewModelBase node = null;
            if (shape is TableShape)
            {
                var table = new TableModel();
                if (info["Content"] != null)
                    table.Content = info["Content"].ToString();
                if (info["MyIsCollapsed"] != null)
                    table.IsCollapsed = bool.Parse(info["MyIsCollapsed"].ToString());
                node = table;
            }
            else
            {
                var row = new RowModel();
                if (info["ColumnName"] != null)
                    row.ColumnName = info["ColumnName"].ToString();
                if (info["DataType"] != null)
                    row.DataType = (DataType)Enum.Parse(typeof(DataType), info["DataType"].ToString(), true);
                node = row;
            }
            if (info["MyPosition"] != null)
                node.Position = Utils.ToPoint(info["MyPosition"].ToString()).Value;

            if (info[this.NodeUniqueIdKey] != null)
            {
                var nodeUniquekey = info[this.NodeUniqueIdKey].ToString();
                if (!this.CachedNodes.ContainsKey(nodeUniquekey))
                {
                    this.CachedNodes.Add(nodeUniquekey, node);
                }
            }

            return node;
        }

        public override void AddNode(NodeViewModelBase node)
        {
            if (!this.InternalItems.Contains(node))
                base.AddNode(node);
        }
    }
}
