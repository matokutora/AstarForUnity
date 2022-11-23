namespace AstarSystem.Data
{
    /// <summary>
    /// �e�m�[�h�̃f�[�^�N���X
    /// </summary>
    public class NodeData
    {
        // X���W
        public int X { get; private set; }

        // Y���W
        public int Y { get; private set; }

        // ���R�X�g
        public int ActualCost { get; private set; }

        // ����R�X�g
        public int EstimateCost { get; private set; }

        // ���v�R�X�g
        public int TotalCost => ActualCost + EstimateCost;

        // �e�m�[�h
        public NodeData ParentNode { get; private set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="x">X���W</param>
        /// <param name="y">Y���W</param>
        /// <param name="parentNode">�e�m�[�h</param>
        public NodeData(int x, int y, NodeData parentNode)
        {
            X = x;
            Y = y;
            ParentNode = parentNode;
        }

        /// <summary>
        /// �X�R�A�̐ݒ�
        /// </summary>
        /// <param name="actualCost">���R�X�g</param>
        /// <param name="estimateCost">����R�X�g</param>
        public void SetScore(int actualCost, int estimateCost)
        {
            ActualCost = actualCost;
            EstimateCost = estimateCost;
        }
    }
}
