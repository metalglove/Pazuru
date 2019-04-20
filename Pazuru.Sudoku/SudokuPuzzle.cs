using Pazuru.Domain;
using Pazuru.Sudoku.Rules;

namespace Pazuru.Sudoku
{
    public sealed class SudokuPuzzle : Puzzle
    {
        public override int Length => 9;
        public override string Description =>
            "A puzzle in which missing numbers are to be filled into a 9 by 9 grid " +
            "of squares which are subdivided into 3 by 3 boxes so that every row, every column, " +
            "and every box contains the numbers 1 through 9.";

        public SudokuPuzzle(PuzzleState puzzleState) : base(puzzleState)
        {
            AddRule(new IsNumberUniqueInBothRowAndColumnSudokuRule(this));
            AddRule(new IsNumberUniqueIn3By3BoxSudokuRule(this));
        }
    }
}
