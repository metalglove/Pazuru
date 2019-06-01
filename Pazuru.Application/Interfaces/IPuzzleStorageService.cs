using System.Threading.Tasks;
using Pazuru.Application.DTOs;

namespace Pazuru.Application.Interfaces
{
    public interface IPuzzleStorageService
    {
        Task<SolvedPuzzles> GetPreviouslySolvedPuzzles();
        Task SavePuzzle(PuzzleDto puzzleDto);
    }
}
