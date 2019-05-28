using System.Collections.Generic;
using System.Linq;

namespace Pazuru.Sudoku.DLX
{
    public readonly struct Solution
    {
        public IEnumerable<int> RowIndexes { get; }

        internal Solution(IEnumerable<int> rowIndexes)
        {
            RowIndexes = rowIndexes.OrderBy(rowIndex => rowIndex);
        }
    }
}
