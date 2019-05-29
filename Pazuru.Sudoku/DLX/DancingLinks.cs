using System.Collections.Generic;
using System.Linq;
using Pazuru.Sudoku.DLX.EnumerableArrayAdapter;

namespace Pazuru.Sudoku.DLX
{
    public static class DancingLinks
    {
        public static IEnumerable<Solution> Solve(int[,] matrix)
        {
            ColumnObject root = BuildInternalStructure(matrix);
            return Search(0, new SearchData(root));
        }

        private static ColumnObject BuildInternalStructure(int[,] data)
        {
            ColumnObject root = new ColumnObject();

            int rowIndex = 0;
            Dictionary<int, ColumnObject> colIndexToListHeader = new Dictionary<int, ColumnObject>();

            foreach (Enumerable2DArrayRow<int> row in new Enumerable2DArray<int>(data))
            {
                DataObject firstDataObjectInThisRow = null;
                int localRowIndex = rowIndex;
                int colIndex = 0;

                foreach (int col in row)
                {
                    if (localRowIndex == 0)
                    {
                        ColumnObject listHeader = new ColumnObject();
                        root.AppendColumnHeader(listHeader);
                        colIndexToListHeader[colIndex] = listHeader;
                    }

                    if (col != 0)
                    {
                        ColumnObject listHeader = colIndexToListHeader[colIndex];
                        DataObject dataObject = new DataObject(listHeader, localRowIndex);

                        if (firstDataObjectInThisRow != null)
                            firstDataObjectInThisRow.AppendToRow(dataObject);
                        else
                            firstDataObjectInThisRow = dataObject;
                    }

                    colIndex++;
                }
                rowIndex++;
            }

            return root;
        }

        private static IEnumerable<Solution> Search(int k, SearchData searchData)
        {
            searchData.IncrementIterationCount();

            if (searchData.Root.NextColumnObject == searchData.Root)
            {
                if (searchData.CurrentSolution.RowIndexes.Any())
                {
                    searchData.IncrementSolutionCount();
                    Solution solution = searchData.CurrentSolution;
                    yield return solution;
                }

                yield break;
            }

            ColumnObject c = ChooseColumnWithLeastRows(searchData.Root);
            CoverColumn(c);

            for (DataObject r = c.Down; r != c; r = r.Down)
            {
                searchData.PushCurrentSolutionRowIndex(r.RowIndex);

                for (DataObject j = r.Right; j != r; j = j.Right)
                    CoverColumn(j.ListHeader);

                IEnumerable<Solution> recursivelyFoundSolutions = Search(k + 1, searchData);
                foreach (Solution solution in recursivelyFoundSolutions)
                    yield return solution;

                for (DataObject j = r.Left; j != r; j = j.Left)
                    UncoverColumn(j.ListHeader);

                searchData.PopCurrentSolutionRowIndex();
            }

            UncoverColumn(c);
        }

        private static ColumnObject ChooseColumnWithLeastRows(ColumnObject root)
        {
            ColumnObject chosenColumn = null;

            for (ColumnObject columnHeader = root.NextColumnObject; columnHeader != root; columnHeader = columnHeader.NextColumnObject)
                if (chosenColumn == null || columnHeader.NumberOfRows < chosenColumn.NumberOfRows)
                    chosenColumn = columnHeader;

            return chosenColumn;
        }

        private static void CoverColumn(ColumnObject c)
        {
            c.UnlinkColumnHeader();

            for (DataObject i = c.Down; i != c; i = i.Down)
                for (DataObject j = i.Right; j != i; j = j.Right)
                    j.ListHeader.UnlinkDataObject(j);
        }

        private static void UncoverColumn(ColumnObject c)
        {
            for (DataObject i = c.Up; i != c; i = i.Up)
                for (DataObject j = i.Left; j != i; j = j.Left)
                    j.ListHeader.RelinkDataObject(j);

            c.RelinkColumnHeader();
        }

        private struct SearchData
        {
            private readonly Stack<int> _currentSolution;

            public ColumnObject Root { get; }
            public int IterationCount { get; private set; }
            public int SolutionCount { get; private set; }

            public Solution CurrentSolution => new Solution(_currentSolution.ToList());

            public SearchData(ColumnObject root)
            {
                Root = root;
                _currentSolution = new Stack<int>();
                IterationCount = 0;
                SolutionCount = 0;
            }

            public void IncrementIterationCount()
            {
                IterationCount++;
            }

            public void IncrementSolutionCount()
            {
                SolutionCount++;
            }

            public void PushCurrentSolutionRowIndex(int rowIndex)
            {
                _currentSolution.Push(rowIndex);
            }

            public void PopCurrentSolutionRowIndex()
            {
                _currentSolution.Pop();
            }
        }
    }
}
