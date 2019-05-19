<template>
    <div id="puzzleElement">
        <Sudoku v-if="puzzleViewModel.name === 'Sudoku'"
                v-bind:sudokuViewModel="createSudokuViewModel()"
                v-bind:sudokuPuzzleService="createSudokuPuzzleService()"/>
        <Hitori v-else-if="puzzleViewModel.name === 'Hitori'"/>
        <EmptyPuzzle v-else />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import Sudoku from '@/components/puzzles/Sudoku/Sudoku.vue';
import Hitori from '@/components/puzzles/Hitori/Hitori.vue';
import EmptyPuzzle from '@/components/puzzles/EmptyPuzzle.vue';
import { PuzzleViewModel } from '@/viewmodels/PuzzleViewModel';
import { SudokuViewModel, Cell } from '../viewmodels/SudokuViewModel';
import { ISudokuPuzzleSevice } from '../services/ISudokuPuzzleService';
import { SudokuPuzzleService } from '../services/SudokuPuzzleService';
import { ICommunicatorService } from '../services/ICommunicatorService';
import { SudokuNumber } from '../types/SudokuNumber';

@Component({
    components: {
        Sudoku,
        Hitori,
        EmptyPuzzle
    }
})
export default class PuzzleView extends Vue {
    private puzzleViewModel: PuzzleViewModel = { name: 'Sudoku', puzzleState: [] };
    @Prop() private communicatorService!: ICommunicatorService;

    //#region Sudoku
    private createSudokuPuzzleService(): ISudokuPuzzleSevice {
        const sudokuPuzzleService: ISudokuPuzzleSevice = new SudokuPuzzleService(this.communicatorService);
        return sudokuPuzzleService;
    }
    private createSudokuViewModel(): SudokuViewModel {
        return { puzzleState: this.createPuzzleState(), moves: [], puzzleLength: 9 };
    }
    //#endregion

    private createPuzzleState(): Cell[] {
        const cells: Cell[] = [];
        for (let i = 0; i < 9; i++) {
            for (let j = 0; j < 9; j++) {
                const cell = this.getCell(i, j);
                cells.push(cell);
            }
        }
        return cells;
    }
    private getCell(row: number, column: number): Cell {
        const puzzleStateString: string =
        '0340070080800650000003000702000007007100' +
        '40096005000001050002000000170060600900430';
        const sudokuNumber: SudokuNumber = (+puzzleStateString.charAt(row * 9 + column)) as SudokuNumber;
        return { row, column, number: sudokuNumber, editable: sudokuNumber === 0 };
    }
}
</script>