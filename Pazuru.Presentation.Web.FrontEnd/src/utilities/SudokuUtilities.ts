import { Cell } from '@/viewmodels/SudokuViewModel';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';

export class SudokuUtilities {
  public static getCellAtIndex(sudokuPuzzlestate: SudokuPuzzleState, row: number, column: number): Cell {
    return sudokuPuzzlestate.cells[(row - 1) * 9 + (column - 1)];
  }
  public static getIndex(row: number, column: number): number {
    return (row - 1) * 9 + (column - 1);
  }
}
