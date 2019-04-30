using Pazuru.Domain;
using System;
using System.Text;

namespace Pazuru.Sudoku
{
    public sealed class SudokuPrinter : PuzzlePrinter<SudokuPuzzle>
    {
        public override string Print(SudokuPuzzle puzzle)
        {
            StringBuilder stringBuilder = new StringBuilder();
            const int deciderWidth = 2;
            int width = puzzle.Size;
            int length = puzzle.Size;
            int decider = width * deciderWidth * 2;
            int newDecider = (decider - 6) / 2;
            stringBuilder.Append($"{CellVerticalLine.ToString().PadRight(newDecider, ' ')}Sudoku{"".PadRight(newDecider, ' ')}");
            stringBuilder.Append($"{CellVerticalLine}");
            stringBuilder.AppendLine();
            string splitter = CellVerticalJointLeft.ToString().PadRight(stringBuilder.Length - 3, CellHorizontalLine);
            stringBuilder.Append(CellVerticalJointLeft);
            for (int j = 0; j < width; j++)
            {
                stringBuilder.Append($"{CellHorizontalLine}".PadRight(deciderWidth * 2 - 1, CellHorizontalLine) + CellHorizontalJointTop);
            }
            stringBuilder.Insert(0, splitter + CellRightTop + Environment.NewLine);
            stringBuilder.Remove(0, 1);
            stringBuilder.Insert(0, CellLeftTop);
            stringBuilder.Replace(CellHorizontalJointTop, CellVerticalJointRight, stringBuilder.Length - 1, 1);
            stringBuilder.AppendLine();

            for (int x = 0; x < length; x++)
            {
                int y = 0;
                for (; y < width; y++)
                {
                    stringBuilder.Append(puzzle[x, y] == default
                        ? $"{CellVerticalLine}".PadRight(deciderWidth * 2, ' ')
                        : $"{CellVerticalLine}".PadRight(deciderWidth, ' ') +
                          $"{puzzle[x, y]}".PadRight(deciderWidth, ' '));
                }
                if (x >= length - 1 && y >= width - 1)
                {
                    stringBuilder.AppendLine(CellVerticalLine.ToString());
                    continue;
                }
                stringBuilder.AppendLine(CellVerticalLine.ToString());
                stringBuilder.Append($"{CellVerticalJointLeft}".PadRight(deciderWidth * 2, CellHorizontalLine));
                for (int j = 1; j < width; j++)
                {
                    stringBuilder.Append($"{CellTJoint}".PadRight(deciderWidth * 2, CellHorizontalLine));
                }
                stringBuilder.AppendLine(CellVerticalJointRight.ToString());
            }

            stringBuilder.Append(CellLeftBottom);
            for (int j = 0; j < width; j++)
            {
                stringBuilder.Append($"{CellHorizontalLine}".PadRight(deciderWidth * 2 - 1, CellHorizontalLine) + CellHorizontalJointBottom);
            }
            stringBuilder.Replace(CellHorizontalJointBottom, CellRightBottom, stringBuilder.Length - 1, 1);
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }
    }
}
