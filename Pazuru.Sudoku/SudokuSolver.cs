using Pazuru.Domain;
using System.Collections.Generic;

namespace Pazuru.Sudoku
{
    public sealed class SudokuSolver : PuzzleSolver<SudokuPuzzle>
    {
        public SudokuSolver(SudokuPuzzle puzzle) : base(puzzle)
        {

        }

        public override bool Solve()
        {
            IsSolved = !GridHasEmptyCell() || RecursiveSolve();
            return IsSolved;
        }
        private bool RecursiveSolve()
        {
            if (!TryFindEmptyCell(out int row, out int column))
            {
                return true;
            }

            for (int number = 1; number <= 9; number++)
            {
                SudokuMove sudokuMove = new SudokuMove(row, column, number);
                if (!sudokuMove.Execute(Puzzle)) continue;
                AddPuzzleState();
                if (RecursiveSolve())
                {
                    return true;
                }

                sudokuMove.Undo(Puzzle);
                AddPuzzleState();
            }

            return false;
        }
        private bool TryFindEmptyCell(out int row, out int column)
        {
            for (int i = 0; i < Puzzle.Length; i++)
            {
                for (int j = 0; j < Puzzle.Length; j++)
                {
                    if (Puzzle[i, j].Equals(0))
                    {
                        row = i;
                        column = j;
                        return true;
                    }
                }
            }
            row = 0;
            column = 0;
            return false;
        }
        private bool GridHasEmptyCell()
        {
            HashSet<int> index = new HashSet<int>();
            for (int i = 0; i < Puzzle.Length; i++)
            {
                for (int j = 0; j < Puzzle.Length; j++)
                {
                    index.Add(Puzzle[i, j]);
                }
            }
            return index.Contains(0);
        }
    }
}
