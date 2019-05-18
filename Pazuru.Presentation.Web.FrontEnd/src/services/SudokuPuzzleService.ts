import { ISudokuPuzzleSevice } from './ISudokuPuzzleService';
import { ICommunicatorService } from './ICommunicatorService';
import { SudokuViewModel } from '@/viewmodels/SudokuViewModel';

export class SudokuPuzzleService implements ISudokuPuzzleSevice {
    private communicatorService!: ICommunicatorService;
    // private sudokuStateListener: SudokuStateListener | undefined;

    constructor(communicatorService: ICommunicatorService) {
        this.communicatorService = communicatorService;
    }

    public generateSudoku(): SudokuViewModel {
        throw new Error('Method not implemented.');
    }

    public solveSudoku(sudokuViewModel: SudokuViewModel): void {
        // if (this.sudokuStateListener == undefined) {
        //     this.sudokuStateListener = new SudokuStateListener(this, 'sudokuState')
        // }
    }
}
