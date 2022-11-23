using System.Collections.Generic;
using System.Linq;

namespace AstarSystem
{
    /// <summary>
    /// Astar探索を行う際のMapデータの作成クラス
    /// </summary>
    public class AstarMapSetting
    {
        class MapData
        {
            /// <summary>
            /// 初期化
            /// </summary>
            /// <param name="id">対象データ</param>
            /// <param name="cost">実コスト</param>
            public MapData(int id, int cost)
            {
                ID = id;
                Cost = cost;
            }

            public int ID { get; private set; }
            public int Cost { get; private set; }
        }

        List<MapData> _mapDataList = new List<MapData>();

        /// <summary>
        /// データの生成
        /// </summary>
        /// <param name="id">Dataの対象</param>
        /// <param name="cost">探索時の実コスト</param>
        /// <returns></returns>
        public AstarMapSetting CreateMapData(int id, int cost)
        {
            MapData data = new MapData(id, cost);
            _mapDataList.Add(data);

            return this;
        }

        /// <summary>
        /// Costの取得
        /// </summary>
        /// <param name="id">指定したデータ</param>
        /// <returns>実コスト</returns>
        public int GetCost(int id)
        {
            try
            {
                return _mapDataList.First(m => m.ID == id).Cost;
            }
            catch
            {
                return 1;
            }
        }
    }
}
