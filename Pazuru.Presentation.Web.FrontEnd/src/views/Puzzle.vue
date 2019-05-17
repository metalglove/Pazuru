<template >
    <Sudoku v-if="puzzleViewModel.name === 'Sudoku'" 
            v-bind:sudokuViewModel="sudokuViewModel"/>
    <Hitori v-else-if="puzzleViewModel.name === 'Hitori'"/>
    <EmptyPuzzle v-else />
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Sudoku from '@/components/puzzles/Sudoku/Sudoku.vue';
import Hitori from '@/components/puzzles/Hitori/Hitori.vue';
import EmptyPuzzle from '@/components/puzzles/EmptyPuzzle.vue';
import { PuzzleViewModel } from '@/viewmodels/PuzzleViewModel';
import { SudokuViewModel, Cell } from '../viewmodels/SudokuViewModel';

@Component({
    components: {
        Sudoku,
        Hitori,
        EmptyPuzzle
    }
})
export default class PuzzleView extends Vue {
    private puzzleViewModel: PuzzleViewModel = { name: 'Sudoku', puzzleState: [] };
    private sudokuViewModel: SudokuViewModel = this.loadSudokuViewModel();

    private loadSudokuViewModel(): SudokuViewModel {
        return { puzzleState: this.createPuzzleState(), moves: [], puzzleLength: 9 };
    }
    private createPuzzleState(): Cell[] {
        const cells: Cell[] = [];
        for (let i = 0; i < 9; i++) {
            for (let j = 0; j < 9; j++) {
                const cell = this.getCell(i, j);
                cells.push(cell);
            }
        }
        // console.log(cells);
        return cells;
    }
    private getCell(row: number, column: number): Cell {
        const irow: number = row;
        const icolumn: number = column;
        const puzzleStateString: string =
        '0340070080800650000003000702000007007100' +
        '40096005000001050002000000170060600900430';
        const sudokuNumber: number = +puzzleStateString.charAt(irow * icolumn);
        return {
            row: irow,
            column: icolumn,
            number: sudokuNumber,
            editable: sudokuNumber === 0
        };
    }
}
</script>