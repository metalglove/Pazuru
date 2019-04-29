using Pazuru.Domain;
using System;

namespace Pazuru.Sudoku.Rules
{
    public sealed class IsNumberUniqueIn3By3BoxSudokuRule : PuzzleRule<SudokuPuzzle>
    {
        public IsNumberUniqueIn3By3BoxSudokuRule(SudokuPuzzle sudoku) : base(sudoku)
        {

        }

        public override bool IsValid<TPuzzle>(PuzzleMove<TPuzzle> puzzleMove)
        {
            if (puzzleMove is SudokuMove sudokuMove)
            {
                int sqrt = (int)Math.Sqrt(Puzzle.Length);
                int x = sudokuMove.Row - sudokuMove.Row % sqrt;
                int y = sudokuMove.Column - sudokuMove.Column % sqrt;
                for (int i = x; i < x + sqrt; i++)
                    for (int j = y; j < y + sqrt; j++)
                        if (Puzzle[i, j].Equals(sudokuMove.Number))
                            return false;
                return true;
            }
            else
                throw new Exception("Invalid PuzzleMove type for Sudoku");
        }
    }
}
