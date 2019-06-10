using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Application.Services;
using Pazuru.Domain;
using Pazuru.Domain.Exceptions;
using Pazuru.Sudoku;

namespace Pazuru.Tests.Services
{
    [TestClass]
    public class SudokuPuzzleServiceTests
    {
        private static IPuzzleService<SudokuPuzzle> _sudokuPuzzleService;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            PuzzleGenerator<SudokuPuzzle> sudokuGenerator = new SudokuGenerator();
            PuzzlePrinter<SudokuPuzzle> sudokuPrinter = new SudokuPrinter();
            _sudokuPuzzleService = new SudokuPuzzleService(sudokuGenerator, sudokuPrinter);
        }

        [TestMethod]
        public void Generate_Should_Return_A_Solvable_Puzzle()
        {
            SudokuPuzzle puzzle = _sudokuPuzzleService.Generate();
            PuzzleSolveDto solveDto = _sudokuPuzzleService.Solve(puzzle);
            Assert.IsTrue(solveDto.Success);
        }

        [TestMethod]
        public void Solve_Should_Return_A_Solved_PuzzleState_Equal_To_Expected_Solution()
        {
            const string solution = "534297618187465329962381574246819753718543296395726841459632187823174965671958432";
            SudokuPuzzle puzzle = CreateValidSudokuPuzzle();
            PuzzleSolveDto solveDto = _sudokuPuzzleService.Solve(puzzle);
            Assert.AreEqual(solution, solveDto.SolvedState.ToString());
            Assert.IsTrue(solveDto.PuzzleStates.Count == 6599);
        }

        [TestMethod]
        public void Print_Should_Return_A_String_Equal_To_Expected_Print()
        {
            const string expectedPrint =
@"┌───────────────────────────────────┐
│              Sudoku               │
├───┬───┬───┬───┬───┬───┬───┬───┬───┤
│ 5 │ 3 │ 4 │ 2 │ 9 │ 7 │ 6 │ 1 │ 8 │
├───┼───┼───┼───┼───┼───┼───┼───┼───┤
│ 1 │ 8 │ 7 │ 4 │ 6 │ 5 │ 3 │ 2 │ 9 │
├───┼───┼───┼───┼───┼───┼───┼───┼───┤
│ 9 │ 6 │ 2 │ 3 │ 8 │ 1 │ 5 │ 7 │ 4 │
├───┼───┼───┼───┼───┼───┼───┼───┼───┤
│ 2 │ 4 │ 6 │ 8 │ 1 │ 9 │ 7 │ 5 │ 3 │
├───┼───┼───┼───┼───┼───┼───┼───┼───┤
│ 7 │ 1 │ 8 │ 5 │ 4 │ 3 │ 2 │ 9 │ 6 │
├───┼───┼───┼───┼───┼───┼───┼───┼───┤
│ 3 │ 9 │ 5 │ 7 │ 2 │ 6 │ 8 │ 4 │ 1 │
├───┼───┼───┼───┼───┼───┼───┼───┼───┤
│ 4 │ 5 │ 9 │ 6 │ 3 │ 2 │ 1 │ 8 │ 7 │
├───┼───┼───┼───┼───┼───┼───┼───┼───┤
│ 8 │ 2 │ 3 │ 1 │ 7 │ 4 │ 9 │ 6 │ 5 │
├───┼───┼───┼───┼───┼───┼───┼───┼───┤
│ 6 │ 7 │ 1 │ 9 │ 5 │ 8 │ 4 │ 3 │ 2 │
└───┴───┴───┴───┴───┴───┴───┴───┴───┘
";
            SudokuPuzzle puzzle = CreateValidSudokuPuzzle();
            _sudokuPuzzleService.Solve(puzzle);
            string actualPrint = _sudokuPuzzleService.Print(puzzle);
            Assert.AreEqual(expectedPrint, actualPrint);
        }

        [TestMethod]
        public void Verify_Should_Return_28_CorrectIndexes()
        {
            PuzzleToVerifyDto puzzleToVerifyDto = new PuzzleToVerifyDto
            {
                CurrentPuzzleState =
                    "034007008080065666666300070200000700710040096005000001050002000000170060600900430",
                OriginalPuzzleState =
                    "034007008080065000000300070200000700710040096005000001050002000000170060600900430"
            };
            PuzzleVerifyDto puzzleVerifyDto = _sudokuPuzzleService.Verify(puzzleToVerifyDto);
            Assert.IsTrue(puzzleVerifyDto.CorrectIndexes.Count == 28);
        }

        [TestMethod]
        public void Verify_Should_Fail_When_CurrentPuzzleState_Is_Empty()
        {
            const string expectedMessage = "PuzzleState is invalid.";
            PuzzleToVerifyDto puzzleToVerifyDto = new PuzzleToVerifyDto
            {
                CurrentPuzzleState = string.Empty,
                OriginalPuzzleState =
                    "034007008080065000000300070200000700710040096005000001050002000000170060600900430"
            };
            PuzzleVerifyDto puzzleVerifyDto = _sudokuPuzzleService.Verify(puzzleToVerifyDto);
            Assert.IsFalse(puzzleVerifyDto.Success);
            Assert.AreEqual(expectedMessage, puzzleVerifyDto.Message);
        }

        [TestMethod]
        public void Verify_Should_Fail_When_OriginalPuzzleState_Is_Empty()
        {
            PuzzleToVerifyDto puzzleToVerifyDto = new PuzzleToVerifyDto
            {
                CurrentPuzzleState = "034007008080065666666300070200000700710040096005000001050002000000170060600900430",
                OriginalPuzzleState = string.Empty
            };
            PuzzleVerifyDto puzzleVerifyDto = _sudokuPuzzleService.Verify(puzzleToVerifyDto);
            Assert.IsFalse(puzzleVerifyDto.Success);
        }

        [TestMethod]
        public void Verify_Should_Fail_When_OriginalPuzzleState_Length_Is_Not_Equal_To_81()
        {
            PuzzleToVerifyDto puzzleToVerifyDto = new PuzzleToVerifyDto
            {
                CurrentPuzzleState = "034007008080065000000300070200000700710040096005000001050002000000170060600900430",
                OriginalPuzzleState = "03400700808006500000030007020000072000000170060600900430"
            };
            PuzzleVerifyDto puzzleVerifyDto = _sudokuPuzzleService.Verify(puzzleToVerifyDto);
            Assert.IsFalse(puzzleVerifyDto.Success);
        }

        [TestMethod]
        public void Verify_Should_Fail_When_CurrentPuzzleState_Length_Is_Not_Equal_To_81()
        {
            PuzzleToVerifyDto puzzleToVerifyDto = new PuzzleToVerifyDto
            {
                CurrentPuzzleState = "03400700808006500000030007020000070430",
                OriginalPuzzleState = "034007008080065000000300070200000700710040096005000001050002000000170060600900430"
            };
            PuzzleVerifyDto puzzleVerifyDto = _sudokuPuzzleService.Verify(puzzleToVerifyDto);
            Assert.IsFalse(puzzleVerifyDto.Success);
        }

        [TestMethod]
        public void Verify_Should_Fail_When_OriginalPuzzleState_Is_Not_Solvable()
        {
            SudokuPuzzle sudoku = CreateInValidSudokuPuzzle();
            PuzzleToVerifyDto puzzleToVerifyDto = new PuzzleToVerifyDto
            {
                CurrentPuzzleState = sudoku.PuzzleState.ToString(),
                OriginalPuzzleState = sudoku.PuzzleState.ToString()
            };
            PuzzleVerifyDto puzzleVerifyDto = _sudokuPuzzleService.Verify(puzzleToVerifyDto);
            Assert.IsFalse(puzzleVerifyDto.Success);
        }

        [TestMethod]
        public void Solve_Should_Fail_When_PuzzleState_Is_Not_Solvable()
        {
            SudokuPuzzle sudoku = CreateInValidSudokuPuzzle();
            PuzzleSolveDto solveDto = _sudokuPuzzleService.Solve(sudoku);
            Assert.IsFalse(solveDto.Success);
        }

        [TestMethod]
        public void GetSolvedState_Should_Throw_PuzzleIsNotYetSolvedException_If_Solve_Has_Not_Been_Called()
        {
            SudokuPuzzle sudoku = CreateValidSudokuPuzzle();
            SudokuSolver sudokuSolver = new SudokuSolver(sudoku);
            Assert.ThrowsException<PuzzleIsNotYetSolvedException>(() => sudokuSolver.GetSolvedState());
        }

        private static SudokuPuzzle CreateValidSudokuPuzzle()
        {
            // 270034009805200074040705020100000900000000000300900052402800000050090260000001048
            // 276134589835269174941785623184652937529347816367918452412876395758493261693521748
            byte[] grid = Encoding.Default.GetBytes("034007008080065000000300070200000700710040096005000001050002000000170060600900430");
            PuzzleState puzzleState = new PuzzleState(grid);
            SudokuPuzzle sudoku = new SudokuPuzzle(puzzleState);
            return sudoku;
        }

        private static SudokuPuzzle CreateInValidSudokuPuzzle()
        {
            byte[] grid = Encoding.Default.GetBytes("828282828282828282828280900000000000000000000000000000000000000000000000000000000");
            PuzzleState puzzleState = new PuzzleState(grid);
            SudokuPuzzle sudoku = new SudokuPuzzle(puzzleState);
            return sudoku;
        }
    }
}
