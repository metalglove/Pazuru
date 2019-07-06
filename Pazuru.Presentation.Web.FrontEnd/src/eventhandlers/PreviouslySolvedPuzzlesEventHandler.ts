import { EventHandler, EventCallBack, EventHandlerDestructor } from '@/services/ICommunicatorService';
import { PreviouslySolvedPuzzlesViewModel } from '@/viewmodels/PreviouslySolvedPuzzlesViewModel';
import { PreviouslySolvedPuzzlesEvent } from './PreviouslySolvedPuzzlesEvent';

export class PreviouslySolvedPuzzlesEventHandler extends EventHandler {
  public callback!: EventCallBack;
  private eventHandlerDestructor!: EventHandlerDestructor;
  private previouslySolvedPuzzlesViewModel!: PreviouslySolvedPuzzlesViewModel;

  constructor(
    eventHandlerDestructor: EventHandlerDestructor,
    previouslySolvedPuzzlesViewModel: PreviouslySolvedPuzzlesViewModel) {
    super('previouslySolvedPuzzlesRequest');
    this.eventHandlerDestructor = eventHandlerDestructor;
    this.previouslySolvedPuzzlesViewModel = previouslySolvedPuzzlesViewModel;
    this.callback = (data: any) => this.setPreviouslySolvedPuzzles(data);
  }

  private setPreviouslySolvedPuzzles(data: PreviouslySolvedPuzzlesEvent): void {
    console.log('Set the prviously solved puzzles.');
    console.log(data);
    this.previouslySolvedPuzzlesViewModel.previouslySolvedPuzzles = data.puzzles;
    this.eventHandlerDestructor(this);
  }
}
