﻿using Pazuru.Domain;
using System.Linq;

namespace Pazuru.Sudoku
{
    public sealed class SudokuMove : PuzzleMove<SudokuPuzzle>
    {
        public int Column { get; }
        public int Row { get; }
        public int Number { get; }
        public int NumberBefore { get; private set; }

        public SudokuMove(int row, int column, int number)
        {
            Column = column;
            Row = row;
            Number = number;
        }

        public override bool Execute(SudokuPuzzle puzzle)
        {
            IsValid = puzzle.PuzzleRules.All(rule => rule.IsValid(this));
            if (!IsValid)
                return false;
            NumberBefore = puzzle[Row, Column];
            puzzle.ExecuteMove(this);
            return IsValid;
        }

        public override void Undo(SudokuPuzzle puzzle)
        {
            if (!IsValid)
                return;
            puzzle.UndoMove(this);
        }
    }
}
