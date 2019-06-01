import { ISudokuPuzzleSevice } from './ISudokuPuzzleService';
import { ICommunicatorService } from './ICommunicatorService';
import { SudokuStateChangeEventHandler } from '@/eventhandlers/SudokuStateChangeEventHandler';
import { GenerateSudokuPuzzleEventHandler } from '@/eventhandlers/GenerateSudokuPuzzleEventHandler';
import { RootState } from '@/store/RootState';
import { VerifySudokuPuzzleEventHandler } from '@/eventhandlers/VerifySudokuPuzzleEventHandler';
import { SudokuUtilities } from '@/utilities/SudokuUtilities';

export class SudokuPuzzleService implements ISudokuPuzzleSevice {
  private communicatorService!: ICommunicatorService;
  private state!: RootState;

  constructor(communicatorService: ICommunicatorService, state: RootState) {
    this.communicatorService = communicatorService;
    this.state = state;
  }

  public verifySudoku(): void {
    const sudokuPuzzleState = (this.state.sudokuViewModel.sudokuPuzzleState!);
    const verifySudokuPuzzleEventHandler: VerifySudokuPuzzleEventHandler =
      new VerifySudokuPuzzleEventHandler(
        this.communicatorService.eventHandlerDestructor(),
        sudokuPuzzleState);
    this.communicatorService.addEventHandler(verifySudokuPuzzleEventHandler);
    this.communicatorService.emit('sudokuVerifyRequest',
    {
      asString: sudokuPuzzleState.asString,
      currentPuzzle: SudokuUtilities.toString(sudokuPuzzleState)
    });
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
