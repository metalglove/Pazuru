using Pazuru.Domain;
using System;

namespace Pazuru.Hitori
{
    public sealed class HitoriMove : PuzzleMove<HitoriPuzzle>
    {
        public int Row { get; }
        public int Column { get; }
        public HitoriMoveColorKey HitoriMoveColorKey { get; }
        public HitoriMoveColorKey HitoriMoveColorKeyBefore { get; }

        public HitoriMove(int row, int column, HitoriMoveColorKey hitoriMoveColorKey)
        {
            Row = row;
            Column = column;
            HitoriMoveColorKey = hitoriMoveColorKey;
        }

        public override bool Execute(HitoriPuzzle puzzle)
        {
            throw new NotImplementedException();
        }

        public override void Undo(HitoriPuzzle puzzle)
        {
            throw new NotImplementedException();
        }
    }
}
