using Pazuru.Domain;
using System;
using System.ComponentModel;

namespace Pazuru.Hitori
{
    public sealed class HitoriMove : PuzzleMove<HitoriPuzzle>
    {
        public int Row { get; }
        public int Column { get; }
        public HitoriMoveColourKey HitoriMoveColourKey { get; }
        public HitoriMoveColourKey HitoriMoveColourKeyBefore { get; }

        public HitoriMove(int row, int column, HitoriMoveColourKey hitoriMoveColourKey)
        {
            Row = row;
            Column = column;
            HitoriMoveColourKey = hitoriMoveColourKey;
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

    public enum HitoriMoveColourKey
    {
        [Description("Undecided")]
        Grey = 'G',
        [Description("Eliminated")]
        Black = 'B',
        [Description("In final solution")]
        White = 'W'
    }
}
