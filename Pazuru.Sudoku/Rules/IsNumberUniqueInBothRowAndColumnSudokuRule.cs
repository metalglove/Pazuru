using Pazuru.Domain;

namespace Pazuru.Sudoku.Rules
{
    public sealed class IsNumberUniqueInBothRowAndColumnSudokuRule : PuzzleRule<SudokuMove, SudokuPuzzle>
    {
        public IsNumberUniqueInBothRowAndColumnSudokuRule(SudokuPuzzle sudoku) : base(sudoku)
        {

        }

        public override bool IsValid(SudokuMove sudokuMove)
        {
            for (int i = 0; i < Puzzle.Size; i++)
                if (Puzzle[sudokuMove.Row, i].Equals(sudokuMove.Number) || Puzzle[i, sudokuMove.Column].Equals(sudokuMove.Number))
                    return false;
            return true;
        }
    }
}
