using Pazuru.Domain;
using System;
using System.Linq;

namespace Pazuru.Hitori
{
    public sealed class HitoriMove : PuzzleMove<HitoriPuzzle>
    {
        public int Row { get; }
        public int Column { get; }
        public HitoriMoveColorKey HitoriMoveColorKey { get; }
        public HitoriMoveColorKey HitoriMoveColorKeyBefore { get; private set; }
        public bool IsUndone { get; private set; }
        public bool IsExecuted { get; private set; }

        public HitoriMove(int row, int column, HitoriMoveColorKey hitoriMoveColorKey)
        {
            Row = row;
            Column = column;
            HitoriMoveColorKey = hitoriMoveColorKey;
        }

        public override bool Execute(HitoriPuzzle puzzle)
        {
            if (IsExecuted)
                throw new Exception("Move is already executed.");
            IsValid = puzzle.PuzzleRules.All(rule => rule.IsValid(this));
            IsExecuted = true;
            if (!IsValid)
                return false;
            HitoriMoveColorKeyBefore = puzzle.GetColorKey(Row, Column);
            if (!puzzle.ExecuteMove(this))
                throw new Exception("Failed to execute move");
            return IsValid;
        }

        public override void Undo(HitoriPuzzle puzzle)
        {
            if (!IsExecuted)
                throw new Exception("Move is not yet executed.");
            if (IsUndone)
                throw new Exception("Move is already undone.");
            if (!puzzle.UndoMove(this))
                throw new Exception("Failed to undo move");
            IsUndone = true;
        }
    }
}
