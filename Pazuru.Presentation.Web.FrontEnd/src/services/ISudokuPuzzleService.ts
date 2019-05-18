import { SudokuViewModel } from '@/viewmodels/SudokuViewModel';

export interface ISudokuPuzzleSevice {
    generateSudoku(): SudokuViewModel;
    solveSudoku(sudokuViewModel: SudokuViewModel): void;
}
