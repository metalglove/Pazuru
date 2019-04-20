using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Domain;
using Pazuru.Sudoku;
using System.Collections.Generic;

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
