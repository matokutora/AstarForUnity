namespace AstarSystem.Data
{
    /// <summary>
    /// 各ノードのデータクラス
    /// </summary>
    public class NodeData
    {
        // X座標
        public int X { get; private set; }

        // Y座標
        public int Y { get; private set; }

        // 実コスト
        public int ActualCost { get; private set; }

        // 推定コスト
        public int EstimateCost { get; private set; }

        // 合計コスト
        public int TotalCost => ActualCost + EstimateCost;

        // 親ノード
        public NodeData ParentNode { get; private set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="parentNode">親ノード</param>
        public NodeData(int x, int y, NodeData parentNode)
        {
            X = x;
            Y = y;
            ParentNode = parentNode;
        }

        /// <summary>
        /// スコアの設定
        /// </summary>
        /// <param name="actualCost">実コスト</param>
        /// <param name="estimateCost">推定コスト</param>
        public void SetScore(int actualCost, int estimateCost)
        {
            ActualCost = actualCost;
            EstimateCost = estimateCost;
        }
    }
}
