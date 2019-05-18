<template>
    <table class="sudoku-table">
        <tbody>
          <tr v-for="(row) in sudokuViewModel.puzzleLength" 
              v-bind:key="row">
            <SudokuCell v-for="(column) in sudokuViewModel.puzzleLength"
                        v-bind:key="getIndex(row, column)" 
                        v-bind:sudokuCell="getSudokuCell(row, column)" />
          </tr>
        </tbody>
      </table>
</template>
<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { SudokuViewModel, Cell } from '@/viewmodels/SudokuViewModel';
import SudokuCell from './SudokuCell.vue';
import { ISudokuPuzzleSevice } from '../../../services/ISudokuPuzzleService';

@Component({
    components: {
        SudokuCell
    }
})
export default class Sudoku extends Vue {
    @Prop() private sudokuViewModel!: SudokuViewModel;
    private sudokuPuzzleService!: ISudokuPuzzleSevice;

    private getSudokuCell(row: number, column: number): Cell {
        const index: number = this.getIndex(row, column);
        const cell: Cell = this.sudokuViewModel.puzzleState[index];
        return cell;
    }

    private getIndex(row: number, column: number): number {
      return (row - 1) * this.sudokuViewModel.puzzleLength + (column - 1);
    }
}
</script>

<style scoped>
.sudoku-table {
  border: 3px solid black;
  margin-left: auto;
  margin-right: auto;
  font-size: 16pt;
}
</style>
