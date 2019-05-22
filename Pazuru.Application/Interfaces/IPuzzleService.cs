using Pazuru.Application.DTOs;
using Pazuru.Domain;

namespace Pazuru.Application.Interfaces
{
    public interface IPuzzleService<TPuzzle> where TPuzzle : Puzzle
    {
        TPuzzle Generate();
        PuzzleVerifyDto Verify(PuzzleToVerifyDto puzzle);
        PuzzleSolveDto Solve(TPuzzle puzzle);
        string Print(TPuzzle puzzle);
    }
}
