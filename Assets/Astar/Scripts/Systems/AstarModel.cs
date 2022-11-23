using System.Linq;
using AstarSystem.Data;
using UnityEngine;

namespace AstarSystem
{
    /// <summary>
    /// Astar�T�����s������
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
        /// �쐬���ꂽ�T������
        /// </summary>
        public SearchData SearchData { get; private set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="xLength">Map�̉��̒���</param>
        /// <param name="yLenght">Map�̏c�̒���</param>
        /// <param name="map">�T�����s��Map</param>
        public AstarModel(int xLength, int yLenght, int[,] map)
        {
            _xLength = xLength;
            _yLength = yLenght;
            _map = map;

            _nodeStockData = new NodeStockData();
        }

        /// <summary>
        /// �S�[���n�_�̐ݒ�
        /// </summary>
        /// <param name="endX">X���W</param>
        /// <param name="endY">Y���W</param>
        /// <returns></returns>
        public AstarModel SetEndIndex(int endX, int endY)
        {
            _endX = endX;
            _endY = endY;

            return this;
        }

        /// <summary>
        /// �ʉ߂ł���f�[�^�̐ݒ�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AstarModel SetOpenIndex(int[] data)
        {
            _openIndexArray = data;

            return this;
        }

        /// <summary>
        /// Setting�f�[�^�̐ݒ�
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public AstarModel SetMapSetting(AstarMapSetting setting)
        {
            _setting = setting;

            return this;
        }

        /// <summary>
        /// �T�����Ɏ΂߂����e����
        /// </summary>
        /// <returns></returns>
        public AstarModel AttributeAiagonal()
        {
            _attributeDiagonal = true;

            return this;
        }

        /// <summary>
        /// �T��
        /// </summary>
        /// <param name="startX">�T�����J�n����X���W</param>
        /// <param name="startY">�T�����J�n����Y���W</param>
        public void OnExecute(int startX, int startY)
        {
            // �X�^�[�g�n�_�̐ݒ�
            NodeData node = CreateNodeData(startX, startY);
            AddOpenNode(node);

            Open(node);
            AddCloseNode(node);

            //�@�T���̎��s
            OnSearch();

            // �S�[���̎擾
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
                        // �΂߂̋��e
                        if (!_attributeDiagonal)
                        {
                            // �l�����݂̂����e
                            if (x == baseNode.X - 1 && y == baseNode.Y - 1) continue;
                            if (x == baseNode.X - 1 && y == baseNode.Y + 1) continue;
                            if (x == baseNode.X + 1 && y == baseNode.Y - 1) continue;
                            if (x == baseNode.X + 1 && y == baseNode.Y + 1) continue;
                        }

                        // Open�ɂ��Ă��悢���̔���
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

        // �T�����ʂ̍쐬
        void CreateSearchData(NodeData node)
        {
            SearchData = new SearchData(_xLength, _yLength, node);
        }

        // NodeData�̍쐬
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
