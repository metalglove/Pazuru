using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pazuru.Domain;
using Pazuru.Sudoku;

namespace Pazuru.Tests.Puzzles
{
    [TestClass]
    public class SudokuPuzzleTests
    {
        [TestMethod]
        public void Puzzle_Of_Type_SudokuPuzzle_Property_Name_Should_Return_SudokuPuzzle()
        {
            const string expected = "SudokuPuzzle";
            Puzzle sudokuPuzzle = new SudokuGenerator().Generate();
            Assert.AreEqual(expected, sudokuPuzzle.Name);
        }

        [TestMethod]
        public void SudokuPuzzle_Description_Equals_Expected()
        {
            const string expected = "A puzzle in which missing numbers are to be filled into a 9 by 9 grid " +
                                    "of squares which are subdivided into 3 by 3 boxes so that every row, every column, " +
                                    "and every box contains the numbers 1 through 9.";
            Puzzle sudokuPuzzle = new SudokuGenerator().Generate();
            Assert.AreEqual(expected, sudokuPuzzle.Description);
        }

        [TestMethod]
        public void Solve_On_A_Solved_SudokuPuzzle_Should_Return_True()
        {
            SudokuPuzzle sudokuPuzzle = new SudokuGenerator().Generate();
            using (SudokuSolver sudokuSolver = new SudokuSolver(sudokuPuzzle))
            {
                sudokuSolver.Solve();
                Assert.IsTrue(sudokuSolver.Solve());
            }
        }

        [TestMethod]
        public void ExecuteMove_With_Invalid_Move_Should_Not_Alter_PuzzleState()
        {
            SudokuPuzzle sudokuPuzzle = new SudokuGenerator().Generate();
            SudokuMove sudokuMove = new SudokuMove(1, 1, 9);
            Assert.IsFalse(sudokuPuzzle.ExecuteMove(sudokuMove));
        }

        [TestMethod]
        public void UndoMove_With_Not_Executed_Move_Should_Not_Alter_PuzzleState()
        {
            SudokuPuzzle sudokuPuzzle = new SudokuGenerator().Generate();
            SudokuMove sudokuMove = new SudokuMove(1, 1, 9);
            Assert.IsFalse(sudokuPuzzle.ExecuteMove(sudokuMove));
        }
    }
}
