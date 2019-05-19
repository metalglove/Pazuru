import { SudokuViewModel } from '@/viewmodels/SudokuViewModel';
import { PuzzleViewModel } from '@/viewmodels/PuzzleViewModel';

export interface RootState {
  sudokuPuzzleLength: number;
  sudokuViewModel: SudokuViewModel;
  puzzleViewModel: PuzzleViewModel;
  puzzles: string[];
}
