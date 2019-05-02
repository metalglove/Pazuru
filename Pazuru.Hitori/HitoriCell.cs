namespace Pazuru.Hitori
{
    public readonly ref struct HitoriCell
    {

        private readonly HitoriMoveColorKey? _up;
        private readonly HitoriMoveColorKey? _down;
        private readonly HitoriMoveColorKey? _right;
        private readonly HitoriMoveColorKey? _left;
        public HitoriMoveColorKey ColorKey { get; }
        public int Number { get; }

        public HitoriCell(HitoriPuzzle hitoriPuzzle, int currentRow, int currentColumn)
        {
            Number = hitoriPuzzle[currentRow, currentColumn];
            ColorKey = hitoriPuzzle.GetColorKey(currentRow, currentColumn);
            _up = currentRow > 0
                ? (HitoriMoveColorKey?)hitoriPuzzle.GetColorKey(currentRow - 1, currentColumn)
                : null;
            _down = currentRow < hitoriPuzzle.Size - 1
                ? (HitoriMoveColorKey?)hitoriPuzzle.GetColorKey(currentRow + 1, currentColumn)
                : null;
            _left = currentColumn > 0
                ? (HitoriMoveColorKey?)hitoriPuzzle.GetColorKey(currentRow, currentColumn - 1)
                : null;
            _right = currentColumn < hitoriPuzzle.Size - 1
                ? (HitoriMoveColorKey?)hitoriPuzzle.GetColorKey(currentRow, currentColumn + 1)
                : null;
        }

        public bool IsConnectedByEitherHorizontalOrVertical(HitoriMoveColorKey colorKey)
        {
            return _up == colorKey ||
                   _down == colorKey ||
                   _left == colorKey ||
                   _right == colorKey;
        }
    }
}
