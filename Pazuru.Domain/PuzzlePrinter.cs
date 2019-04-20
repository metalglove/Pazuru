namespace Pazuru.Domain
{
    public abstract class PuzzlePrinter<TPuzzle> where TPuzzle : Puzzle
    {
        protected const char CellLeftTop = '┌';
        protected const char CellRightTop = '┐';
        protected const char CellLeftBottom = '└';
        protected const char CellRightBottom = '┘';
        protected const char CellHorizontalJointTop = '┬';
        protected const char CellHorizontalJointBottom = '┴';
        protected const char CellVerticalJointLeft = '├';
        protected const char CellTJoint = '┼';
        protected const char CellVerticalJointRight = '┤';
        protected const char CellHorizontalLine = '─';
        protected const char CellVerticalLine = '│';

        public abstract string Print(TPuzzle puzzle);
    }
}
