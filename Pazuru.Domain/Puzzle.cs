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
            protected set
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
        public abstract bool ExecuteMove<TPuzzle>(PuzzleMove<TPuzzle> puzzleMove) where TPuzzle : Puzzle;
        public abstract bool UndoMove<TPuzzle>(PuzzleMove<TPuzzle> puzzleMove) where TPuzzle : Puzzle;
        public PuzzleState CopyPuzzleState()
        {
            byte[] bytes = new byte[PuzzleState.Value.Length];
            Buffer.BlockCopy(PuzzleState.Value, 0, bytes, 0, PuzzleState.Value.Length);
            return new PuzzleState(bytes);
        }
        
        public void Dispose()
        {
            puzzleRules.Clear();
        }
    }
}
