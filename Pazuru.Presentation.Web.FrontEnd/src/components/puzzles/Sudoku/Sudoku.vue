<template>
  <div>
    <table v-if="sudokuViewmodel.sudokuPuzzleStateIsGenerated" class="sudoku-table">
      <tbody>
        <tr v-for="(row) in getSudokuPuzzleLength" 
            v-bind:key="row">
          <SudokuCell v-for="(column) in getSudokuPuzzleLength"
                      v-bind:key="getIndex(row, column)" 
                      v-bind:sudokuCell="getSudokuCell(row, column)" />
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { SudokuViewModel, Cell } from '@/viewmodels/SudokuViewModel';
import SudokuCell from './SudokuCell.vue';
import { ISudokuPuzzleSevice } from '../../../services/ISudokuPuzzleService';
import { mapState, StoreOptions, mapGetters } from 'vuex';
import { SudokuPuzzleState } from '../../../models/Sudoku/SudokuPuzzleState';
import { RootState } from '@/store/RootState';
import { SudokuUtilities } from '@/utilities/SudokuUtilities';

@Component({
  components: {
    SudokuCell
  },
  computed: {
    ...mapState<RootState>({
      sudokuViewmodel: (state): SudokuViewModel => state.sudokuViewModel
    }),
    ...mapGetters([
      'getSudokuCell',
      'getSudokuPuzzleLength'
    ])
  }
})
export default class Sudoku extends Vue {
  private getIndex(row: number, column: number): number {
    return SudokuUtilities.getIndex(row, column);
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
