import { SolvedPuzzle } from '@/models/SolvedPuzzle';

export interface PreviouslySolvedPuzzlesViewModel {
  previouslySolvedPuzzles: SolvedPuzzle[];
  selectedPuzzle: 'Sudoku' | 'Hitori' | 'None';
}
