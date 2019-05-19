import { Cell } from '@/viewmodels/SudokuViewModel';

export class SudokuPuzzleState {
  public asString!: string;
  public cells!: Cell[];

  constructor(sudokuPuzzleStateString: string, cells: Cell[]) {
    this.asString = sudokuPuzzleStateString;
    this.cells = cells;
  }
}
