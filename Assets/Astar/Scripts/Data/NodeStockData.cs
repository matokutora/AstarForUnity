using System.Collections.Generic;
using System.Linq;

namespace AstarSystem.Data
{
    /// <summary>
    /// 探索中のノードを保存するクラス
    /// </summary>
    public class NodeStockData
    {
        /// <summary>
        /// 通過可能なノード管理するクラス
        /// </summary>
        public class OpenData
        {
            List<NodeData> _openNodeDataList = new List<NodeData>();

            public void AddNode(NodeData addNode)
            {
                NodeData node = _openNodeDataList.FirstOrDefault(n => n.X == addNode.X && n.Y == addNode.Y);

                if (node == null)
                {
                    _openNodeDataList.Add(addNode);
                    return;
                }

                if (node.EstimateCost > addNode.EstimateCost)
                {
                    node = addNode;
                }
            }

            public void RemoveNode(NodeData node)
            {
                _openNodeDataList.Remove(node);
            }

            public NodeData GetNode()
            {
                return _openNodeDataList.OrderBy(n => n.TotalCost).FirstOrDefault();
            }
        }

        /// <summary>
        /// 通過不可なノード管理するノード
        /// </summary>
        public class CloseData
        {
            List<NodeData> _closeNodeDataList = new List<NodeData>();

            public void AddNode(NodeData node)
            {
                _closeNodeDataList.Add(node);
            }

            public NodeData GetNode(int x, int y)
            {
                return _closeNodeDataList.FirstOrDefault(n => n.X == x && n.Y == y);
            }
        }

        public OpenData Open { get; private set; } = new OpenData();

        public CloseData Close { get; private set; } = new CloseData();
    }
}
