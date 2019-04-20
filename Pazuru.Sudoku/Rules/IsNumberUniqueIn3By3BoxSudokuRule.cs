using Pazuru.Domain;
using System;

namespace Pazuru.Sudoku.Rules
{
    public sealed class IsNumberUniqueIn3By3BoxSudokuRule : PuzzleRule<SudokuPuzzle>
    {
        public IsNumberUniqueIn3By3BoxSudokuRule(SudokuPuzzle sudoku) : base(sudoku)
        {

        }

        public override bool IsValid(int row, int column, int number)
        {
            int sqrt = (int)Math.Sqrt(Puzzle.Length);
            int x = row - row % sqrt;
            int y = column - column % sqrt;
            for (int i = x; i < x + sqrt; i++)
                for (int j = y; j < y + sqrt; j++)
                    if (Puzzle[i, j].Equals(number))
                        return false;
            return true;
        }
    }
}
