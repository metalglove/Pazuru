import { Cell } from '@/viewmodels/SudokuViewModel';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';
import { SudokuNumber } from '@/types/SudokuNumber';

export class SudokuUtilities {
  public static toString(sudokuPuzzlestate: SudokuPuzzleState): string {
    return sudokuPuzzlestate.cells.map((cell) => cell.number).join('');
  }
  public static getCellAtIndex(sudokuPuzzlestate: SudokuPuzzleState, row: number, column: number): Cell {
    return sudokuPuzzlestate.cells[(row - 1) * 9 + (column - 1)];
  }
  public static getIndex(row: number, column: number): number {
    return (row - 1) * 9 + (column - 1);
  }
  public static createPuzzleState(puzzleStateString: string): Cell[] {
    const cells: Cell[] = [];
    for (let i = 0; i < 9; i++) {
      for (let j = 0; j < 9; j++) {
        const cell = this.getCell(i, j, puzzleStateString);
        cells.push(cell);
      }
    }
    return cells;
  }
  public static getCell(row: number, column: number, puzzleStateString: string): Cell {
    const sudokuNumber: SudokuNumber = (+puzzleStateString.charAt(row * 9 + column)) as SudokuNumber;
    return { row, column, number: sudokuNumber, editable: sudokuNumber === 0, verified: false };
  }
}
