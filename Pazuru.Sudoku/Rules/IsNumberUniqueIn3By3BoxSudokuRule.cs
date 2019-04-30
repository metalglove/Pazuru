using Pazuru.Domain;
using System;

namespace Pazuru.Sudoku.Rules
{
    public sealed class IsNumberUniqueIn3By3BoxSudokuRule : PuzzleRule<SudokuMove, SudokuPuzzle>
    {
        public IsNumberUniqueIn3By3BoxSudokuRule(SudokuPuzzle sudoku) : base(sudoku)
        {

        }

        public override bool IsValid(SudokuMove sudokuMove)
        {
            int sqrt = (int)Math.Sqrt(Puzzle.Size);
            int x = sudokuMove.Row - sudokuMove.Row % sqrt;
            int y = sudokuMove.Column - sudokuMove.Column % sqrt;
            for (int i = x; i < x + sqrt; i++)
                for (int j = y; j < y + sqrt; j++)
                    if (Puzzle[i, j].Equals(sudokuMove.Number))
                        return false;
            return true;
        }
    }
}
