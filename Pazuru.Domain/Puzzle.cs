using System;
using System.Collections.Generic;

namespace Pazuru.Domain
{
    public abstract class Puzzle : IDisposable
    {
        private readonly List<IPuzzleRule> puzzleRules = new List<IPuzzleRule>();
        public IReadOnlyCollection<IPuzzleRule> PuzzleRules => puzzleRules;
        public string Name => GetType().Name;
        public abstract string Description { get; }
        public PuzzleState PuzzleState { get; }
        public abstract int Length { get; }
        public virtual int this[int row, int column]
        {
            get
            {
                int index = (row * Length) + column;
                byte byteValue = Buffer.GetByte(PuzzleState.Value, index);
                return byteValue - 48;
            }
            set
            {
                byte byteValue = (byte)(value + 48);
                int index = (row * Length) + column;
                Buffer.SetByte(PuzzleState.Value, index, byteValue);
            }
        }

        protected Puzzle(PuzzleState puzzleState)
        {
            PuzzleState = puzzleState;
        }

        protected void AddRule(IPuzzleRule puzzleRule)
        {
            puzzleRules.Add(puzzleRule);
        }

        public void Dispose()
        {
            puzzleRules.Clear();
        }
    }
}
