import { EventHandler, EventCallBack, EventHandlerDestructor } from '@/services/ICommunicatorService';
import { PreviouslySolvedPuzzlesViewModel } from '@/viewmodels/PreviouslySolvedPuzzlesViewModel';

export class PreviouslySolvedPuzzlesEventHandler extends EventHandler {
    public callback!: EventCallBack;
    private eventHandlerDestructor!: EventHandlerDestructor;
    private previouslySolvedPuzzlesViewModel!: PreviouslySolvedPuzzlesViewModel;

    constructor(eventHandlerDestructor: EventHandlerDestructor, previouslySolvedPuzzlesViewModel: PreviouslySolvedPuzzlesViewModel) {
        super('previouslySolvedPuzzlesRequest');
        this.eventHandlerDestructor = eventHandlerDestructor;
        this.previouslySolvedPuzzlesViewModel = previouslySolvedPuzzlesViewModel;
        this.callback = (data: any) => this.setPreviouslySolvedPuzzles(data);
    }

    private setPreviouslySolvedPuzzles(data: PreviouslySolvedPuzzlesEvent) : void {
        console.log('Set the prviously solved puzzles.');
        //previouslySolvedPuzzlesRequest
        this.eventHandlerDestructor(this);

    }
}