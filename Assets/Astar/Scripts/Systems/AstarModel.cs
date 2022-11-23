using System.Linq;
using AstarSystem.Data;
using UnityEngine;

namespace AstarSystem
{
    /// <summary>
    /// Astar探索を行う実体
    /// </summary>
    public class AstarModel
    {
        int _xLength = 0;
        int _yLength = 0;
        int[,] _map = null;

        int _endX = 0;
        int _endY = 0;
        int[] _openIndexArray = null;

        bool _attributeDiagonal = false;

        NodeStockData _nodeStockData;
        AstarMapSetting _setting;

        /// <summary>
        /// 作成された探索結果
        /// </summary>
        public SearchData SearchData { get; private set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="xLength">Mapの横の長さ</param>
        /// <param name="yLenght">Mapの縦の長さ</param>
        /// <param name="map">探索を行うMap</param>
        public AstarModel(int xLength, int yLenght, int[,] map)
        {
            _xLength = xLength;
            _yLength = yLenght;
            _map = map;

            _nodeStockData = new NodeStockData();
        }

        /// <summary>
        /// ゴール地点の設定
        /// </summary>
        /// <param name="endX">X座標</param>
        /// <param name="endY">Y座標</param>
        /// <returns></returns>
        public AstarModel SetEndIndex(int endX, int endY)
        {
            _endX = endX;
            _endY = endY;

            return this;
        }

        /// <summary>
        /// 通過できるデータの設定
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AstarModel SetOpenIndex(int[] data)
        {
            _openIndexArray = data;

            return this;
        }

        /// <summary>
        /// Settingデータの設定
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public AstarModel SetMapSetting(AstarMapSetting setting)
        {
            _setting = setting;

            return this;
        }

        /// <summary>
        /// 探索時に斜めを許容する
        /// </summary>
        /// <returns></returns>
        public AstarModel AttributeAiagonal()
        {
            _attributeDiagonal = true;

            return this;
        }

        /// <summary>
        /// 探索
        /// </summary>
        /// <param name="startX">探索を開始するX座標</param>
        /// <param name="startY">探索を開始するY座標</param>
        public void OnExecute(int startX, int startY)
        {
            // スタート地点の設定
            NodeData node = CreateNodeData(startX, startY);
            AddOpenNode(node);

            Open(node);
            AddCloseNode(node);

            //　探索の実行
            OnSearch();

            // ゴールの取得
            node = _nodeStockData.Close.GetNode(_endX, _endY);
            if (node != null)
            {
                CreateSearchData(node);
                return;
            }
            
        }

        void OnSearch()
        {
            NodeData node = _nodeStockData.Open.GetNode();

            if (node == null || node.X == _endX && node.Y == _endY)
            {
                AddCloseNode(node);
                return;
            }

            Open(node);
            AddCloseNode(node);

            OnSearch();
        }

        void Open(NodeData baseNode)
        {
            for (int x = baseNode.X - 1; x <= baseNode.X + 1; x++)
            {
                for (int y = baseNode.Y - 1; y <= baseNode.Y + 1; y++)
                {
                    if (x >= 0 && x < _xLength && y >= 0 && y < _yLength)
                    {
                        // 斜めの許容
                        if (!_attributeDiagonal)
                        {
                            // 四方向のみを許容
                            if (x == baseNode.X - 1 && y == baseNode.Y - 1) continue;
                            if (x == baseNode.X - 1 && y == baseNode.Y + 1) continue;
                            if (x == baseNode.X + 1 && y == baseNode.Y - 1) continue;
                            if (x == baseNode.X + 1 && y == baseNode.Y + 1) continue;
                        }

                        // Openにしてもよいかの判定
                        if (!_openIndexArray.Any(n => n == _map[x, y])) continue;
                        if (_nodeStockData.Close.GetNode(x, y) != null) continue;

                        int cost = _setting != null ? _setting.GetCost(_map[x, y]) : 1;
                        
                        int actualCost = baseNode.ActualCost + cost;
                        NodeData node = CreateNodeData(x, y, baseNode, actualCost);
                        AddOpenNode(node);
                    }
                }
            }
        }

        // 探索結果の作成
        void CreateSearchData(NodeData node)
        {
            SearchData = new SearchData(_xLength, _yLength, node);
        }

        // NodeDataの作成
        NodeData CreateNodeData(int x, int y, NodeData parentNode = null, int actualCost = 0)
        {
            int diffX = _endX - x;
            int diffY = _endY - y;
            int stimateCost = _attributeDiagonal ? stimateCost = Mathf.Min(diffX, diffY) : diffX + diffY;

            NodeData node = new NodeData(x, y, parentNode);
            node.SetScore(actualCost, stimateCost);

            return node;
        }

        void AddOpenNode(NodeData node)
        {
            _nodeStockData.Open.AddNode(node);
        }

        void AddCloseNode(NodeData node)
        {
            if (node == null) return;

            _nodeStockData.Open.RemoveNode(node);
            _nodeStockData.Close.AddNode(node);
        }
    }
}
