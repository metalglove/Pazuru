import { SudokuNumber } from '@/types/SudokuNumber';

export class SudokuPuzzleMove {
  public row!: number;
  public column!: number;
  public puzzleState!: string;
  public numberBefore!: SudokuNumber;
  public numberAfter!: SudokuNumber;

  constructor(
    row: number,
    column: number,
    puzzleState: string,
    numberBefore: SudokuNumber,
    numberAfter: SudokuNumber) {
    this.row = row;
    this.column = column;
    this.puzzleState = puzzleState;
    this.numberAfter = numberAfter;
    this.numberBefore = numberBefore;
  }
}
