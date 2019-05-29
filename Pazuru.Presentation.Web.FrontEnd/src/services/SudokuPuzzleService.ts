import { ISudokuPuzzleSevice } from './ISudokuPuzzleService';
import { ICommunicatorService, EventHandler, EventHandlerDestructor } from './ICommunicatorService';
import { SudokuStateChangeEventHandler } from '@/eventhandlers/SudokuStateChangeEventHandler';
import { GenerateSudokuPuzzleEventHandler } from '@/eventhandlers/GenerateSudokuPuzzleEventHandler';
import { RootState } from '@/store/RootState';

export class SudokuPuzzleService implements ISudokuPuzzleSevice {
  private communicatorService!: ICommunicatorService;
  private state!: RootState;

  constructor(communicatorService: ICommunicatorService, state: RootState) {
    this.communicatorService = communicatorService;
    this.state = state;
  }

  public generateSudoku(): void {
    const sudokuGeneratePuzzleRequest: GenerateSudokuPuzzleEventHandler =
      new GenerateSudokuPuzzleEventHandler(
        this.communicatorService.eventHandlerDestructor(),
        this.state.sudokuViewModel);
    this.communicatorService.addEventHandler(sudokuGeneratePuzzleRequest);
    this.communicatorService.emit('sudokuGeneratePuzzleRequest', undefined);
  }

  public solveSudoku(): void {
    const sudokuPuzzleState = (this.state.sudokuViewModel.sudokuPuzzleState!);
    const sudokuStateHandler: SudokuStateChangeEventHandler =
      new SudokuStateChangeEventHandler(
        this.communicatorService.eventHandlerDestructor(),
        sudokuPuzzleState);
    this.communicatorService.addEventHandler(sudokuStateHandler);
    this.communicatorService.emit('sudokuSolvePuzzleRequest', { asString: sudokuPuzzleState.asString });
  }
}
