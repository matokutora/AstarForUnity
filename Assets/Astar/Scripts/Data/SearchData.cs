using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AstarSystem.Data
{
    public struct SearchData
    {
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="xLength">Mapの横の長さ</param>
        /// <param name="yLength">Mapの縦の長さ</param>
        /// <param name="node">ゴールしたNode</param>
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
        /// 探索結果の配列の長さを取得
        /// </summary>
        public int Length => _routeList.Count;

        /// <summary>
        /// 探索結果のルートを取得
        /// </summary>
        /// <returns></returns>
        public List<Vector2Int> GetRoute()
        {
            return _routeList;
        }

        /// <summary>
        /// ゴール地点の取得
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetEnd()
        {
            return _routeList.Last();
        }

        /// <summary>
        /// 探索されたルートを新たなデータに書き換える
        /// </summary>
        /// <param name="map">更新するMap</param>
        /// <param name="updateData">更新するデータ</param>
        /// <returns>更新されたデータ</returns>
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