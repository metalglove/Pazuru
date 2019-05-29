using System;

namespace Pazuru.Application.DTOs
{
    public class PuzzleDto
    {
        public string PuzzleType { get; set; }
        public string OriginalPuzzleState { get; set; }
        public string SolvedPuzzleState { get; set; }
        public DateTime SolvedDateTime { get; set; }
    }
}
