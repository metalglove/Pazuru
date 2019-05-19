import { ISudokuPuzzleSevice } from './ISudokuPuzzleService';
import { ICommunicatorService } from './ICommunicatorService';
import { SudokuViewModel } from '@/viewmodels/SudokuViewModel';
import { SudokuStateChangeEventHandler } from '@/eventhandlers/SudokuStateChangeEventHandler';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';

export class SudokuPuzzleService implements ISudokuPuzzleSevice {
    private communicatorService!: ICommunicatorService;
    private sudokuStateHandler: SudokuStateChangeEventHandler | undefined;

    constructor(communicatorService: ICommunicatorService) {
        this.communicatorService = communicatorService;
    }

    public generateSudoku(): SudokuViewModel {
        throw new Error('Method not implemented.');
    }

    public solveSudoku(sudokuViewModel: SudokuViewModel): void {
        if (this.sudokuStateHandler === undefined) {
            this.sudokuStateHandler = new SudokuStateChangeEventHandler(sudokuViewModel);
            this.communicatorService.addEventHandler(this.sudokuStateHandler);
            const puzzleState: string = sudokuViewModel.puzzleState.map(xd => xd.number).join('');
            this.communicatorService.emit('sudokuSolveRequest', new SudokuPuzzleState(puzzleState));
        }
    }
}
