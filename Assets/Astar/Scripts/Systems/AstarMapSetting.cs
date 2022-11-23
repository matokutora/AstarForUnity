using System.Collections.Generic;
using System.Linq;

namespace AstarSystem
{
    /// <summary>
    /// Astar�T�����s���ۂ�Map�f�[�^�̍쐬�N���X
    /// </summary>
    public class AstarMapSetting
    {
        class MapData
        {
            /// <summary>
            /// ������
            /// </summary>
            /// <param name="id">�Ώۃf�[�^</param>
            /// <param name="cost">���R�X�g</param>
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
        /// �f�[�^�̐���
        /// </summary>
        /// <param name="id">Data�̑Ώ�</param>
        /// <param name="cost">�T�����̎��R�X�g</param>
        /// <returns></returns>
        public AstarMapSetting CreateMapData(int id, int cost)
        {
            MapData data = new MapData(id, cost);
            _mapDataList.Add(data);

            return this;
        }

        /// <summary>
        /// Cost�̎擾
        /// </summary>
        /// <param name="id">�w�肵���f�[�^</param>
        /// <returns>���R�X�g</returns>
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
