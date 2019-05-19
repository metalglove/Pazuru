import { SudokuNumber } from '@/types/SudokuNumber';

export interface SudokuStateChangeEvent {
    index: number;
    numberAfter: SudokuNumber;
    numberBefore: SudokuNumber;
    changed: boolean;
}