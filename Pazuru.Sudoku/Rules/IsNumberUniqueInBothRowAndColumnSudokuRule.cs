using Pazuru.Domain;

namespace Pazuru.Sudoku.Rules
{
    public sealed class IsNumberUniqueInBothRowAndColumnSudokuRule : PuzzleRule<SudokuPuzzle>
    {
        public IsNumberUniqueInBothRowAndColumnSudokuRule(SudokuPuzzle sudoku) : base(sudoku)
        {

        }

        public override bool IsValid(int row, int column, int number)
        {
            for (int i = 0; i < Puzzle.Length; i++)
                if (Puzzle[row, i].Equals(number) || Puzzle[i, column].Equals(number))
                    return false;
            return true;
        }
    }
}
