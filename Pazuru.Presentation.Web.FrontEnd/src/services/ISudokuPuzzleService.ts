import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';

export interface ISudokuPuzzleSevice {
    generateSudoku(): void;
    solveSudoku(): void;
}
