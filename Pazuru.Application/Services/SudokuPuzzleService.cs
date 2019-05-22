using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Domain;
using Pazuru.Sudoku;
using System.Collections.Generic;
using System.Text;

namespace Pazuru.Application.Services
{
    public class SudokuPuzzleService : IPuzzleService<SudokuPuzzle>
    {
        private readonly PuzzleGenerator<SudokuPuzzle> _sudokuGenerator;
        private readonly PuzzlePrinter<SudokuPuzzle> _sudokuPrinter;

        public SudokuPuzzleService(PuzzleGenerator<SudokuPuzzle> puzzleGenerator, PuzzlePrinter<SudokuPuzzle> sudokuPrinter)
        {
            _sudokuGenerator = puzzleGenerator;
            _sudokuPrinter = sudokuPrinter;
        }

        public SudokuPuzzle Generate()
        {
            return _sudokuGenerator.Generate();
        }

        public PuzzleVerifyDto Verify(PuzzleToVerifyDto puzzleToVerifyDto)
        {
            if (string.IsNullOrWhiteSpace(puzzleToVerifyDto.OriginalPuzzleState) ||
                string.IsNullOrWhiteSpace(puzzleToVerifyDto.CurrentPuzzleState) ||
                puzzleToVerifyDto.OriginalPuzzleState.Length != 81 ||
                puzzleToVerifyDto.CurrentPuzzleState.Length != 81)
                return new PuzzleVerifyDto { Message = "PuzzleState is invalid." };

            byte[] grid = Encoding.Default.GetBytes(puzzleToVerifyDto.OriginalPuzzleState);
            PuzzleState puzzleState = new PuzzleState(grid);
            SudokuPuzzle sudoku = new SudokuPuzzle(puzzleState);
            string actualSolvedPuzzleState;
            using (SudokuSolver sudokuSolver = new SudokuSolver(sudoku))
            {
                if (!sudokuSolver.Solve())
                    return new PuzzleVerifyDto { Message = "Failed to solve puzzle. Verify will not work." };
                actualSolvedPuzzleState = sudokuSolver.GetSolvedState().ToString();
            }

            List<int> correctIndexes = new List<int>();
            List<int> wrongIndexes = new List<int>();
            for (int i = 0; i < puzzleToVerifyDto.OriginalPuzzleState.Length; i++)
            {
                if (actualSolvedPuzzleState[i].Equals(puzzleToVerifyDto.CurrentPuzzleState[i]))
                    correctIndexes.Add(i);
                else
                    wrongIndexes.Add(i);
            }
            return new PuzzleVerifyDto
            {
                Success = true,
                CorrectIndexes = correctIndexes,
                WrongIndexes = wrongIndexes,
                Message = "Successfully verified puzzleState."
            };
        }

        public string Print(SudokuPuzzle puzzle)
        {
            return _sudokuPrinter.Print(puzzle);
        }

        public PuzzleSolveDto Solve(SudokuPuzzle puzzle)
        {
            using (SudokuSolver sudokuSolver = new SudokuSolver(puzzle))
            {
                if (sudokuSolver.Solve())
                    return new PuzzleSolveDto(true, new List<PuzzleState>(sudokuSolver.PuzzleStates), sudokuSolver.GetSolvedState());
            }
            return new PuzzleSolveDto(false, null, new PuzzleState());
        }
    }
}
