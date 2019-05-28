using System;
using Pazuru.Domain;

namespace Pazuru.Sudoku
{
    public sealed class DLXSudokuPuzzleSolver : PuzzleSolver<SudokuPuzzle>
    {
        public DLXSudokuPuzzleSolver(SudokuPuzzle puzzle) : base(puzzle)
        {

        }

        public override bool Solve()
        {
            throw new NotImplementedException();
        }
    }
}
