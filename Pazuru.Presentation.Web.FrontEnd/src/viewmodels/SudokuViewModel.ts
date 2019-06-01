import { SudokuNumber } from '@/types/SudokuNumber';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';

export interface SudokuViewModel {
    moves: Move[];
    sudokuPuzzleStateIsGenerated: boolean;
    sudokuPuzzleState: SudokuPuzzleState | undefined;
}
export interface Move {
    cell: Cell;
    executed: boolean;
    undone: boolean;
}
export interface Cell {
    row: number;
    column: number;
    number: SudokuNumber;
    editable: boolean;
    verified: boolean;
}
