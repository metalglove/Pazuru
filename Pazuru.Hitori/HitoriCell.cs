namespace Pazuru.Hitori
{
    public readonly ref struct ExtendedHitoriCell
    {
        private readonly HitoriCell _cell;
        public HitoriCell? UpCell { get; }
        public HitoriCell? DownCell { get; }
        public HitoriCell? LeftCell { get; }
        public HitoriCell? RightCell { get; }
        public HitoriMoveColorKey ColorKey => (HitoriMoveColorKey)_cell.ColorKey;
        public int Number => _cell.Number;
        public int Row => _cell.Row;
        public int Column => _cell.Column;

        public ExtendedHitoriCell(HitoriPuzzle hitoriPuzzle, int currentRow, int currentColumn)
        {
            _cell = new HitoriCell(hitoriPuzzle, currentRow, currentColumn);
            UpCell = currentRow > 0
                ? (HitoriCell?)new HitoriCell(hitoriPuzzle, currentRow - 1, currentColumn)
                : null;
            DownCell = currentRow < hitoriPuzzle.Size - 1
                ? (HitoriCell?)new HitoriCell(hitoriPuzzle, currentRow + 1, currentColumn)
                : null;
            LeftCell = currentColumn > 0 
                ? (HitoriCell?)new HitoriCell(hitoriPuzzle, currentRow, currentColumn - 1)
                : null;
            RightCell = currentColumn < hitoriPuzzle.Size - 1
                ? (HitoriCell?)new HitoriCell(hitoriPuzzle, currentRow, currentColumn + 1)
                : null;
        }

        public bool IsConnectedByEitherHorizontalOrVertical(HitoriMoveColorKey colorKey)
        {
            return UpCell?.ColorKey == colorKey ||
                   DownCell?.ColorKey == colorKey ||
                   LeftCell?.ColorKey == colorKey ||
                   RightCell?.ColorKey == colorKey;
        }
    }

    public readonly struct HitoriCell
    {
        private readonly Cell _cell;
        public int Number => _cell.Number;
        public int Row => _cell.Row;
        public int Column => _cell.Column;
        public HitoriMoveColorKey ColorKey { get; }

        public HitoriCell(HitoriPuzzle hitoriPuzzle, int row, int column)
        {
            _cell = new Cell(hitoriPuzzle[row, column], row, column);
            ColorKey = hitoriPuzzle.GetColorKey(row, column);
        }
    }

    public readonly struct Cell
    {
        public int Number { get; }
        public int Row { get; }
        public int Column { get; }

        public Cell(int number, int row, int column)
        {
            Number = number;
            Row = row;
            Column = column;
        }
    }
}
