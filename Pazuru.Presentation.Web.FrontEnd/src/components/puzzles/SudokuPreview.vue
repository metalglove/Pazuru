<template>
  <div class="wrapper">
    <h1> Sudoku </h1>
    <div class="left">
      <h2> Original </h2>
      <table class="sudoku-table">
      <tbody>
        <tr v-for="(row) in getSudokuPuzzleLength" 
            v-bind:key="row">
          <SudokuCell v-for="(column) in getSudokuPuzzleLength"
                      v-bind:key="getIndex(row, column)" 
                      v-bind:sudokuCell="getSudokuCell(sudokuPreviewViewModel.originalPuzzleState, row, column)" />
        </tr>
      </tbody>
    </table>
    </div>
    <div class="right">
      <h2> Solved </h2>
      <table class="sudoku-table">
      <tbody>
        <tr v-for="(row) in getSudokuPuzzleLength" 
            v-bind:key="row">
          <SudokuCell v-for="(column) in getSudokuPuzzleLength"
                      v-bind:key="getIndex(row, column)" 
                      v-bind:sudokuCell="getSudokuCell(sudokuPreviewViewModel.solvedPuzzleState, row, column)" />
        </tr>
      </tbody>
    </table>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { SudokuViewModel, Cell } from '@/viewmodels/SudokuViewModel';
import { mapState, StoreOptions, mapGetters } from 'vuex';
import { RootState } from '@/store/RootState';
import { SudokuUtilities } from '@/utilities/SudokuUtilities';
import {SudokuPreviewViewModel } from '@/viewmodels/SudokuPreviewViewModel';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';
import SudokuCell from './Sudoku/SudokuCell.vue';

@Component({
  components: {
    SudokuCell
  },
  computed: {
    ...mapGetters([
      'getSudokuPuzzleLength'
    ])
  }
})
export default class SudokuPreview extends Vue {
  @Prop() private sudokuPreviewViewModel!: SudokuPreviewViewModel;

  private getIndex(row: number, column: number): number {
    return SudokuUtilities.getIndex(row, column);
  }

  private getSudokuCell(sudokuPuzzleState: SudokuPuzzleState, row: number, column: number): Cell {
    const cell: Cell = SudokuUtilities.getCellAtIndex(sudokuPuzzleState, row, column);
    cell.editable = false;
    return cell;
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
.wrapper {
  width: 80%;
  margin: 0 auto;
}
.left {
  float: left;
  width: 50%;
}
.right {
  float: left;
  width: 50%;
}
</style>
