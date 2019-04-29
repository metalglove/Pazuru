using Pazuru.Domain;
using System;

namespace Pazuru.Sudoku.Rules
{
    public sealed class IsNumberUniqueInBothRowAndColumnSudokuRule : PuzzleRule<SudokuPuzzle>
    {
        public IsNumberUniqueInBothRowAndColumnSudokuRule(SudokuPuzzle sudoku) : base(sudoku)
        {

        }

        public override bool IsValid<TPuzzle>(PuzzleMove<TPuzzle> puzzleMove)
        {
            if (puzzleMove is SudokuMove sudokuMove)
            {
                for (int i = 0; i < Puzzle.Length; i++)
                    if (Puzzle[sudokuMove.Row, i].Equals(sudokuMove.Number) || Puzzle[i, sudokuMove.Column].Equals(sudokuMove.Number))
                        return false;
                return true;
            }
            else
                throw new Exception("Invalid PuzzleMove type for Sudoku");
        }
    }
}
