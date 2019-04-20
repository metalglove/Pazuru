using System;
using System.Runtime.Serialization;

namespace Pazuru.Domain.Exceptions
{
    public sealed class PuzzleIsNotYetSolvedException : Exception
    {
        public PuzzleIsNotYetSolvedException() : base("Puzzle is not yet solved!")
        {
        }
    }
}
