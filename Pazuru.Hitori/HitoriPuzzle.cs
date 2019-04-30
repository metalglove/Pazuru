﻿using Pazuru.Domain;

namespace Pazuru.Hitori
{
    public sealed class HitoriPuzzle : Puzzle<HitoriMove, HitoriPuzzle>
    {
        public override string Description => 
            "Hitori is a number-elimination puzzle based on a square grid filled with numbers. " +
            "The object is to shade squares so that no number appears in a row or column more than once. " +
            "In addition, shaded (black) squares do not touch each other vertically or horizontally and all " +
            "un-shaded (white) squares create a single continuous area when the puzzle is completed.";
        public override int Length { get; }

        public HitoriPuzzle(PuzzleState puzzleState, int length = 9) : base(puzzleState)
        {
            Length = length;
        }

        public override bool ExecuteMove(HitoriMove hitoriMove)
        {
            throw new System.NotImplementedException();
        }

        public override bool UndoMove(HitoriMove hitoriMove)
        {
            throw new System.NotImplementedException();
        }
    }
}
