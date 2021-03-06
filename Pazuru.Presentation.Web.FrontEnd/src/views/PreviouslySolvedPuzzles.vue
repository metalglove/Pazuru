<template>
  <div class="wrapper">
    <div class="left">
      <div v-for="(puzzle) in filter(previouslySolvedPuzzlesViewModel)" 
           v-bind:key="puzzle.puzzleId">
        <SudokuPreview 
          v-if="puzzle.puzzleType === 'Sudoku'" 
          v-bind:sudokuPreviewViewModel="getSudokuPreviewViewModel(puzzle)" />
      </div>
    </div>
    <div class="right">
      <h1>Filter by puzzle:</h1>
      <select v-model="previouslySolvedPuzzlesViewModel.selectedPuzzle">
        <option v-for="puzzle in puzzles" v-bind:value="puzzle" v-bind:key="puzzle">
          {{ puzzle }}
        </option>
      </select>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { mapGetters, mapState, Store } from 'vuex';
import { RootState } from '@/store/RootState';
import { IPuzzleService } from '../services/IPuzzleService';
import SudokuPreview from '@/components/puzzles/SudokuPreview.vue';
import { SolvedPuzzle } from '../models/SolvedPuzzle';
import { SudokuPreviewViewModel } from '../viewmodels/SudokuPreviewViewModel';
import { SudokuUtilities } from '@/utilities/SudokuUtilities';
import { SudokuPuzzleState } from '@/models/Sudoku/SudokuPuzzleState';
import {PreviouslySolvedPuzzlesViewModel} from '@/viewmodels/PreviouslySolvedPuzzlesViewModel';

@Component({
  components: {
    SudokuPreview
  },
  computed: {
    ...mapState(['previouslySolvedPuzzlesViewModel', 'puzzles'])
  },
  mounted() {
    this.$props.puzzleService.getPreviouslySolvedPuzzles();
  }
})
export default class PreviouslySolvedPuzzlesView extends Vue {
  @Prop() private puzzleService!: IPuzzleService;

  private getSudokuPreviewViewModel(solvedPuzzle: SolvedPuzzle): SudokuPreviewViewModel {
    const sudokuPreviewViewModel: SudokuPreviewViewModel = {
      solvedPuzzle,
      solvedPuzzleState:
        new SudokuPuzzleState(
          solvedPuzzle.solvedPuzzle,
          SudokuUtilities.createPuzzleState(solvedPuzzle.solvedPuzzle)),
      originalPuzzleState:
        new SudokuPuzzleState(
          solvedPuzzle.originalPuzzle,
          SudokuUtilities.createPuzzleState(solvedPuzzle.originalPuzzle))
    };
    return sudokuPreviewViewModel;
  }

  private filter(solvedPuzzlesVM: PreviouslySolvedPuzzlesViewModel): SolvedPuzzle[] {
    if (solvedPuzzlesVM.selectedPuzzle === 'None') {
      return solvedPuzzlesVM.previouslySolvedPuzzles;
    }
    return solvedPuzzlesVM.previouslySolvedPuzzles
      .filter((puzzle) => puzzle.puzzleType === solvedPuzzlesVM.selectedPuzzle);
  }
}
</script>

<style scoped>
.wrapper {
  width: 80%;
  margin: 0 auto;
}
.left {
  float: left;
  width: 75%;
  background-color: gray;
}
.right {
  float: left;
  width: 25%;
  background-color: lightblue;
}
</style>