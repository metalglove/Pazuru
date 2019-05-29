import { IPuzzleService } from './IPuzzleService';
import { ICommunicatorService } from './ICommunicatorService';
import { RootState } from '@/store/RootState';
import { PreviouslySolvedPuzzlesEventHandler } from '@/eventhandlers/PreviouslySolvedPuzzlesEventHandler';

export class PuzzleService implements IPuzzleService {
  private communicatorService!: ICommunicatorService;
  private state!: RootState;

  constructor(communicatorService: ICommunicatorService, state: RootState) {
    this.communicatorService = communicatorService;
    this.state = state;
  }

  public getPreviouslySolvedPuzzles(): void {
    const previouslySolvedPuzzlesEventHandler: PreviouslySolvedPuzzlesEventHandler =
      new PreviouslySolvedPuzzlesEventHandler(
        this.communicatorService.eventHandlerDestructor(),
        this.state.previouslySolvedPuzzlesViewModel);
    this.communicatorService.addEventHandler(previouslySolvedPuzzlesEventHandler);
    this.communicatorService.emit('previouslySolvedPuzzlesRequest', undefined);
  }
}
