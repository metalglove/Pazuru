using System;
using System.Collections.Generic;

namespace Pazuru.Domain
{
    public abstract class Puzzle<TPuzzleMove, TPuzzle> : Puzzle where TPuzzle : Puzzle where TPuzzleMove : PuzzleMove<TPuzzle>
    {
        private readonly List<PuzzleRule<TPuzzleMove, TPuzzle>> _puzzleRules = new List<PuzzleRule<TPuzzleMove, TPuzzle>>();
        public IReadOnlyCollection<PuzzleRule<TPuzzleMove, TPuzzle>> PuzzleRules => _puzzleRules;

        protected Puzzle(PuzzleState puzzleState) : base(puzzleState)
        {

        }

        protected void AddRule(PuzzleRule<TPuzzleMove, TPuzzle> puzzleRule) 
        {
            _puzzleRules.Add(puzzleRule);
        }
        public abstract bool ExecuteMove(TPuzzleMove puzzleMove);
        public abstract bool UndoMove(TPuzzleMove puzzleMove);
        public override void Dispose()
        {
            _puzzleRules.Clear();
        }
    }

    public abstract class Puzzle : IDisposable
    {   
        public string Name => GetType().Name;
        public abstract string Description { get; }
        public PuzzleState PuzzleState { get; }
        public abstract int Size { get; }
        public virtual int this[int row, int column]
        {
            get
            {
                int index = (row * Size) + column;
                byte byteValue = Buffer.GetByte(PuzzleState.Value, index);
                return byteValue - 48;
            }
            protected set
            {
                byte byteValue = (byte)(value + 48);
                int index = (row * Size) + column;
                Buffer.SetByte(PuzzleState.Value, index, byteValue);
            }
        }

        protected Puzzle(PuzzleState puzzleState)
        {
            PuzzleState = puzzleState;
        }

        public PuzzleState CopyPuzzleState()
        {
            byte[] bytes = new byte[PuzzleState.Value.Length];
            Buffer.BlockCopy(PuzzleState.Value, 0, bytes, 0, PuzzleState.Value.Length);
            return new PuzzleState(bytes);
        }

        public abstract void Dispose();
    }
}
