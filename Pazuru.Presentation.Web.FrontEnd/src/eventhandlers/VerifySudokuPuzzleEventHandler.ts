import { EventHandler, EventCallBack, EventHandlerDestructor } from '@/services/ICommunicatorService';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';
import { Cell } from '@/viewmodels/SudokuViewModel';
import { VerifySudokuPuzzleEvent } from './VerifySudokuPuzzleEvent';

export class VerifySudokuPuzzleEventHandler extends EventHandler {
  public callback!: EventCallBack;
  private sudokuPuzzleState!: SudokuPuzzleState;
  private eventHandlerDestructor!: EventHandlerDestructor;

  constructor(eventHandlerDestructor: EventHandlerDestructor, sudokuPuzzleState: SudokuPuzzleState) {
    super('sudokuVerifyRequest');
    this.sudokuPuzzleState = sudokuPuzzleState;
    this.eventHandlerDestructor = eventHandlerDestructor;
    this.callback = (data: any) => this.verifySudokuState(data);
  }

  private verifySudokuState(data: VerifySudokuPuzzleEvent): void {
    this.sudokuPuzzleState.cells.forEach((cell) => {
      cell.verified = false;
    });
    if (data.correctIndexes.length > 0) {
      data.correctIndexes.forEach((num) => {
        const cell: Cell = this.sudokuPuzzleState.cells[num];
        cell.verified = true;
      });
    }
    this.eventHandlerDestructor(this);
  }
}
