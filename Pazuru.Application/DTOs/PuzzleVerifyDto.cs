using System.Collections.Generic;

namespace Pazuru.Application.DTOs
{
    public class PuzzleVerifyDto
    {
        public List<int> CorrectIndexes { get; set; }
        public List<int> WrongIndexes { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
