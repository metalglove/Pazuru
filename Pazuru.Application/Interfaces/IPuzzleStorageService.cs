using System.Collections.Generic;
using System.Threading.Tasks;
using Pazuru.Application.DTOs;

namespace Pazuru.Application.Interfaces
{
    public interface IPuzzleStorageService
    {
        Task<List<PuzzleDto>> GetPreviouslySolvedPuzzles();
    }
}
