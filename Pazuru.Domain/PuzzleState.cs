using System.Text;

namespace Pazuru.Domain
{
    public readonly struct PuzzleState
    {
        public byte[] Value { get; }

        public PuzzleState(byte[] value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Encoding.Default.GetString(Value);
        }
    }
}
