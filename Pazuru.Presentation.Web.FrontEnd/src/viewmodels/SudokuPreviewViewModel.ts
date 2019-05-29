import { SolvedPuzzle } from '@/models/SolvedPuzzle';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';

export interface SudokuPreviewViewModel {
  solvedPuzzle: SolvedPuzzle;
  originalPuzzleState: SudokuPuzzleState;
  solvedPuzzleState: SudokuPuzzleState;
}
