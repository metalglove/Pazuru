using Pazuru.Application.Interfaces;
using Pazuru.Application.DTOs;
using Pazuru.Domain;
using Pazuru.Hitori;
using System.Collections.Generic;

namespace Pazuru.Application.Services
{
    public sealed class HitoriPuzzleService : IPuzzleService<HitoriPuzzle>
    {
        private readonly PuzzleGenerator<HitoriPuzzle> _puzzleGenerator;
        private readonly PuzzlePrinter<HitoriPuzzle> _puzzlePrinter;

        public HitoriPuzzleService(PuzzleGenerator<HitoriPuzzle> puzzleGenerator, PuzzlePrinter<HitoriPuzzle> puzzlePrinter)
        {
            _puzzleGenerator = puzzleGenerator;
            _puzzlePrinter = puzzlePrinter;
        }

        public HitoriPuzzle Generate()
        {
            return _puzzleGenerator.Generate();
        }

        public string Print(HitoriPuzzle puzzle)
        {
            return _puzzlePrinter.Print(puzzle);
        }

        public PuzzleSolveDto Solve(HitoriPuzzle puzzle)
        {
            using (HitoriSolver hitoriSolver = new HitoriSolver(puzzle))
            {
                if (hitoriSolver.Solve())
                    return new PuzzleSolveDto(true, new List<PuzzleState>(hitoriSolver.PuzzleStates), puzzle.PuzzleState);
            }
            return new PuzzleSolveDto(false, null, new PuzzleState());
        }
    }
}
