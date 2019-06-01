export interface SolvedPuzzle {
  _links: Links;
  puzzleId: number;
  puzzleType: 'Sudoku' | 'Hitori';
  solvedPuzzle: string;
  originalPuzzle: string;
}

export interface Self {
  href: string;
  templated: boolean;
}

export interface Links {
  self: Self;
}
