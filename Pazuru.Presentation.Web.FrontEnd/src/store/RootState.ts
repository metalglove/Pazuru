import { SudokuViewModel } from '@/viewmodels/SudokuViewModel';
import { PuzzleViewModel } from '@/viewmodels/PuzzleViewModel';
import { PreviouslySolvedPuzzlesViewModel } from '@/viewmodels/PreviouslySolvedPuzzlesViewModel';

export interface RootState {
  sudokuPuzzleLength: number;
  sudokuViewModel: SudokuViewModel;
  puzzleViewModel: PuzzleViewModel;
  puzzles: string[];
  previouslySolvedPuzzlesViewModel: PreviouslySolvedPuzzlesViewModel;
}
