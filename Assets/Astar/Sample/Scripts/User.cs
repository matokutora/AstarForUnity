using UnityEngine;
using AstarSystem;

namespace AstarSystem.Sample
{
    public class User : MonoBehaviour
    {
        enum DataType
        {
            Load = 0,
            Mountain = 5,

            Wall = 1,

            Start = 2,
            Goal = 3,

            Route = 4,
        }

        class CellData
        {
            public int X { get; set; }
            public int Y { get; set; }
            public GameObject Prefab { get; set; }
        }

        [SerializeField] GameObject _wallPrefabs;
        [SerializeField] bool _attributeAiagonal;

        int[,] _map = new int[,]
        {
        { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
        { 0, 2, 0, 0, 0, 0, 0, 0, 0 ,1 },
        { 0, 0, 0, 0, 1, 1, 1, 1, 0 ,1 },
        { 1, 0, 1, 0, 5, 0, 1, 1, 0 ,1 },
        { 1, 0, 1, 1, 1, 0, 1, 1, 1 ,1 },
        { 1, 0, 0, 0, 1, 5, 0, 0, 1 ,1 },
        { 1, 0, 1, 0, 1, 0, 1, 0, 0 ,1 },
        { 1, 0, 1, 0, 1, 0, 1, 1, 0 ,1 },
        { 1, 0, 1, 0, 1, 0, 0, 0, 0 ,0 },
        { 1, 0, 1, 1, 1, 1, 1, 0, 3 ,0 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0 ,0 },
        };

        CellData[,] _cellDataArray;

        readonly Vector2Int StartCell = new Vector2Int(1, 1);
        readonly Vector2Int EndCell = new Vector2Int(9, 8);

        readonly int XLength = 11;
        readonly int YLength = 10;

        void Start()
        {
            _cellDataArray = new CellData[XLength, YLength];

            OnView();

            AstarMapSetting setting = new AstarMapSetting();
            setting
                .CreateMapData((int)DataType.Load, 1)
                .CreateMapData((int)DataType.Mountain, 3);

            AstarModel astar = new AstarModel(XLength, YLength, _map);
            astar
                .SetEndIndex(EndCell.x, EndCell.y)
                .SetOpenIndex(new int[]
                {
                    (int)DataType.Load,
                    (int)DataType.Mountain,
                    (int)DataType.Start,
                    (int)DataType.Goal
                })
                .SetMapSetting(setting);

            if (_attributeAiagonal)
            {
                astar.AttributeAiagonal();
            }
    
            astar.OnExecute(StartCell.x, StartCell.y);

            _map = astar.SearchData.OverWriteMap(_map, (int)DataType.Route);

            SetData();

            Debug.Log($"Length => {astar.SearchData.Length}");
            Debug.Log($"EndPos => {astar.SearchData.GetEnd()}");
            astar.SearchData.GetRoute().ForEach(r => Debug.Log($"Position {r}"));
        }

        void OnView()
        {
            for (int x = 0; x < XLength; x++)
            {
                for (int y = 0; y < YLength; y++)
                {
                    Transform t = Instantiate(_wallPrefabs).transform;
                    t.name = _wallPrefabs.name;
                    t.position = new Vector2(x, y);

                    CellData data = new CellData
                    {
                        X = x,
                        Y = y,
                        Prefab = t.gameObject
                    };

                    _cellDataArray[x, y] = data;
                }
            }
        }

        void SetData()
        {
            for (int x = 0; x < XLength; x++)
            {
                for (int y = 0; y < YLength; y++)
                {
                    CellData data = _cellDataArray[x, y];
                    SpriteRenderer sprite = data.Prefab.GetComponent<SpriteRenderer>();
                    string path = $"X:{data.X}_Y:{data.Y} => ";
                    switch (_map[x, y])
                    {
                        case (int)DataType.Load:
                            data.Prefab.name = path + DataType.Load.ToString();
                            sprite.color = Color.white;
                            break;

                        case (int)DataType.Wall:
                            data.Prefab.name = path + DataType.Wall.ToString();
                            sprite.color = Color.black;
                            break;

                        case (int)DataType.Start:
                            data.Prefab.name = path + DataType.Start.ToString();
                            sprite.color = Color.green;
                            break;

                        case (int)DataType.Goal:
                            data.Prefab.name = path + DataType.Goal.ToString();
                            sprite.color = Color.blue;
                            break;

                        case (int)DataType.Route:
                            data.Prefab.name = path + DataType.Route.ToString();
                            sprite.color = Color.red;
                            break;

                        case (int)DataType.Mountain:
                            data.Prefab.name = path + DataType.Mountain.ToString();
                            sprite.color = Color.gray;
                            break;
                    }
                }
            }
        }
    }
}
