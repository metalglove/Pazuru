import { EventCallBack, EventHandler, EventHandlerDestructor } from '@/services/ICommunicatorService';
import { SudokuViewModel, Cell } from '@/viewmodels/SudokuViewModel';
import { SudokuPuzzleGenerateEvent } from './SudokuPuzzleGenerateEvent';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';
import { SudokuNumber } from '@/types/SudokuNumber';

export class GenerateSudokuPuzzleEventHandler extends EventHandler {
  public callback!: EventCallBack;
  private sudokuViewModel!: SudokuViewModel;
  private eventHandlerDestructor!: EventHandlerDestructor;

  constructor(eventHandlerDestructor: EventHandlerDestructor, sudokuViewModel: SudokuViewModel) {
    super('sudokuGeneratePuzzleRequest');
    this.sudokuViewModel = sudokuViewModel;
    this.eventHandlerDestructor = eventHandlerDestructor;
    this.callback = (data: any) => this.setSudokuPuzzleState(data);
  }

  private setSudokuPuzzleState(data: SudokuPuzzleGenerateEvent): void {
    console.log('received generated puzzle');
    const puzzleStateCells: Cell[] = this.createPuzzleState(data.puzzleAsString);
    const puzzleState: SudokuPuzzleState = new SudokuPuzzleState(data.puzzleAsString, puzzleStateCells);
    this.sudokuViewModel.sudokuPuzzleState = puzzleState;
    this.sudokuViewModel.sudokuPuzzleStateIsGenerated = true;
    this.eventHandlerDestructor(this);
  }

  private createPuzzleState(puzzleStateString: string): Cell[] {
    const cells: Cell[] = [];
    for (let i = 0; i < 9; i++) {
      for (let j = 0; j < 9; j++) {
        const cell = this.getCell(i, j, puzzleStateString);
        cells.push(cell);
      }
    }
    return cells;
  }

  private getCell(row: number, column: number, puzzleStateString: string): Cell {
    const sudokuNumber: SudokuNumber = (+puzzleStateString.charAt(row * 9 + column)) as SudokuNumber;
    return { row, column, number: sudokuNumber, editable: sudokuNumber === 0 };
  }
}
