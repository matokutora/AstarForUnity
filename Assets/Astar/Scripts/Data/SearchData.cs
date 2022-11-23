using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AstarSystem.Data
{
    public struct SearchData
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="xLength">Map�̉��̒���</param>
        /// <param name="yLength">Map�̏c�̒���</param>
        /// <param name="node">�S�[������Node</param>
        public SearchData(int xLength, int yLength, NodeData node)
        {
            _xLength = xLength;
            _yLength = yLength;

            _routeList = new List<Vector2Int>();

            SetRoute(node);
        }

        int _xLength;
        int _yLength;

        List<Vector2Int> _routeList;

        void SetRoute(NodeData node)
        {
            while (node.ParentNode != null)
            {
                Vector2Int value = new Vector2Int(node.X, node.Y);
                _routeList.Add(value);

                node = node.ParentNode;
            }

            _routeList.Reverse();
        }

        /// <summary>
        /// �T�����ʂ̔z��̒������擾
        /// </summary>
        public int Length => _routeList.Count;

        /// <summary>
        /// �T�����ʂ̃��[�g���擾
        /// </summary>
        /// <returns></returns>
        public List<Vector2Int> GetRoute()
        {
            return _routeList;
        }

        /// <summary>
        /// �S�[���n�_�̎擾
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetEnd()
        {
            return _routeList.Last();
        }

        /// <summary>
        /// �T�����ꂽ���[�g��V���ȃf�[�^�ɏ���������
        /// </summary>
        /// <param name="map">�X�V����Map</param>
        /// <param name="updateData">�X�V����f�[�^</param>
        /// <returns>�X�V���ꂽ�f�[�^</returns>
        public int[,] OverWriteMap(int[,] map, int updateData)
        {
            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    try
                    {
                        Vector2Int value = _routeList.First(map => map.x == x && map.y == y);
                        if (value.x == x && value.y == y)
                        {
                            map[x, y] = updateData;
                        }
                    }
                    catch 
                    { }
                }
            }

            return map;
        }
    }
}