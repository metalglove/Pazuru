import { EventCallBack, EventHandler } from '@/services/ICommunicatorService';
import { SudokuViewModel, Cell } from '@/viewmodels/SudokuViewModel';
import { SudokuStateChangeEvent } from './SudokuStateChangeEvent';

export class SudokuStateChangeEventHandler extends EventHandler {
    public callback!: EventCallBack;
    private sudokuViewModel!: SudokuViewModel;

    constructor(sudokuViewModel: SudokuViewModel) {
        super('sudokuPuzzleStateChange');
        this.sudokuViewModel = sudokuViewModel;
        this.callback = (data: any) => this.updateSudokuState(data);
    }

    private updateSudokuState(data: SudokuStateChangeEvent): void {
        console.log(data);
        if (data.changed) {
            const cell: Cell = this.sudokuViewModel.puzzleState[data.index];
            if (cell.number === data.numberBefore) {
                console.log('i changed');
                cell.number = data.numberAfter;
            } else {
                console.log('i DID NOT change');
            }
        }
    }
}