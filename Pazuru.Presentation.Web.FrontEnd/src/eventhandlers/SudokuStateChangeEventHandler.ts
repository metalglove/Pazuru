import { EventCallBack, EventHandler, EventHandlerDestructor } from '@/services/ICommunicatorService';
import { Cell } from '@/viewmodels/SudokuViewModel';
import { SudokuStateChangeEvent } from './SudokuStateChangeEvent';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';
import { SudokuUtilities } from '@/utilities/SudokuUtilities';
import { SudokuNumber } from '@/types/SudokuNumber';

export class SudokuStateChangeEventHandler extends EventHandler {
  public callback!: EventCallBack;
  private sudokuPuzzleState!: SudokuPuzzleState;
  private eventHandlerDestructor!: EventHandlerDestructor;

  constructor(eventHandlerDestructor: EventHandlerDestructor, sudokuPuzzleState: SudokuPuzzleState) {
    super('sudokuPuzzleStateChange');
    this.sudokuPuzzleState = sudokuPuzzleState;
    this.eventHandlerDestructor = eventHandlerDestructor;
    this.callback = (data: any) => this.updateSudokuState(data);
  }

  private updateSudokuState(data: SudokuStateChangeEvent): void {
    if (data.changed) {
      const cell: Cell = this.sudokuPuzzleState.cells[data.index];
      cell.number = data.numberAfter;
    }
    if (data.lastEvent) {
      console.log(SudokuUtilities.toString(this.sudokuPuzzleState));
      this.eventHandlerDestructor(this);
    }
  }
}
