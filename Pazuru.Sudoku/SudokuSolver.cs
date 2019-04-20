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
            // voor uberhaupt het solven van de puzzel begint kan er al gekeken worden of die al opgelost is.
            IsSolved = !GridHasEmptyCell() || RecursiveSolve();
            return IsSolved;
        }
        private bool RecursiveSolve()
        {
            // vind de row en column van een lege cell in het puzzel grid
            if (!TryFindEmptyCell(out int row, out int column))
            {
                // als er geen is geef true terug
                return true;
            }

            // voor elk getal in de getallen van 1 tot 9
            for (int i = 1; i <= 9; i++)
            {
                // als er geen conflict is voor dit getal op deze row en column,
                if (IsAssignmentValid(row, column, i))
                {
                    // zet het getal in die row en column en ga recursief verder met de rest van de puzzle 
                    this[row, column] = i;
                    if (RecursiveSolve())
                    {
                        //als de recursieve functie succesvol was uitgevoerd dan word true terug gegeven
                        return true;
                    }

                    // als de recursieve functie niet succesvol was uitgevoerd ga naar het volgende getal
                    // en zet de waarde in de cell terug naar 0
                    this[row, column] = 0;
                }
            }

            // als alle getallen zijn geweest en niks heeft gewerkt word false terug gegeven
            return false;
        }
        private bool TryFindEmptyCell(out int row, out int column)
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (this[i, j] == 0)
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
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    index.Add(this[i, j]);
                }
            }
            return index.Contains(0);
        }
    }
}
