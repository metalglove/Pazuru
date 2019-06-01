import { SudokuViewModel } from '@/viewmodels/SudokuViewModel';
import { PuzzleViewModel } from '@/viewmodels/PuzzleViewModel';
import { PreviouslySolvedPuzzlesViewModel } from '@/viewmodels/PreviouslySolvedPuzzlesViewModel';
import { ModalViewModel } from '@/viewmodels/ModalViewModel';

export interface RootState {
  sudokuPuzzleLength: number;
  sudokuViewModel: SudokuViewModel;
  puzzleViewModel: PuzzleViewModel;
  puzzles: string[];
  previouslySolvedPuzzlesViewModel: PreviouslySolvedPuzzlesViewModel;
  modalViewModel: ModalViewModel;
}
