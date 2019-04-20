using Pazuru.Domain;
using System.Collections.Generic;

namespace Pazuru.Application.DTOs
{
    public class PuzzleSolveDto
    {
        public bool Success { get; }
        public IReadOnlyCollection<PuzzleState> PuzzleStates { get; }
        public PuzzleState SolvedState { get; }

        public PuzzleSolveDto(bool success, IReadOnlyCollection<PuzzleState> puzzleStates, PuzzleState solvedState)
        {
            Success = success;
            PuzzleStates = puzzleStates;
            SolvedState = solvedState;
        }
    }
}
