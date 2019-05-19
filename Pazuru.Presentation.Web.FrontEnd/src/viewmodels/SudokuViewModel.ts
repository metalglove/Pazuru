import { SudokuNumber } from '@/types/SudokuNumber';

export interface SudokuViewModel {
    moves: Move[];
    puzzleState: Cell[];
    puzzleLength: number;
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
}
