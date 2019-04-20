using Pazuru.Domain;
using System.Text;

namespace Pazuru.Sudoku
{
    public sealed class SudokuGenerator : PuzzleGenerator<SudokuPuzzle>
    {
        public override SudokuPuzzle Generate()
        {
            // TODO: create random generator
            byte[] grid = Encoding.Default.GetBytes("034007008080065000000300070200000700710040096005000001050002000000170060600900430");
            PuzzleState puzzleState = new PuzzleState(grid);
            SudokuPuzzle sudoku = new SudokuPuzzle(puzzleState);
            return sudoku;
        }
    }
}
