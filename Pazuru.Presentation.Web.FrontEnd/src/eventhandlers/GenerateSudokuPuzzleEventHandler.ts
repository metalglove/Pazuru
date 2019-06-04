import { EventCallBack, EventHandler, EventHandlerDestructor } from '@/services/ICommunicatorService';
import { SudokuViewModel, Cell } from '@/viewmodels/SudokuViewModel';
import { SudokuPuzzleGenerateEvent } from './SudokuPuzzleGenerateEvent';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';
import { SudokuUtilities } from '@/utilities/SudokuUtilities';

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
    const puzzleStateCells: Cell[] = SudokuUtilities.createPuzzleState(data.puzzleAsString);
    const puzzleState: SudokuPuzzleState = new SudokuPuzzleState(data.puzzleAsString, puzzleStateCells);
    const puzzleStateCells2: Cell[] = SudokuUtilities.createPuzzleState(data.puzzleAsString);
    const puzzleState2: SudokuPuzzleState = new SudokuPuzzleState(data.puzzleAsString, puzzleStateCells2);
    this.sudokuViewModel.sudokuPuzzleState = puzzleState;
    this.sudokuViewModel.originalPuzzleState = puzzleState2;
    this.sudokuViewModel.sudokuPuzzleStateIsGenerated = true;
    this.eventHandlerDestructor(this);
  }
}
