import { IPuzzleService } from './IPuzzleService';
import { ICommunicatorService } from './ICommunicatorService';
import { RootState } from '@/store/RootState';

export class PuzzleService implements IPuzzleService {
    private communicatorService!: ICommunicatorService;
    private state!: RootState;

    constructor(communicatorService: ICommunicatorService, state: RootState) {
        this.communicatorService = communicatorService;
        this.state = state;
    }

    public getPreviouslySolvedPuzzles(): void {
        throw new Error("Method not implemented.");
    }

}