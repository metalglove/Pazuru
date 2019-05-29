<template>
  <div class="wrapper">
    <div class="left">
      <Sudoku v-if="puzzleViewModel.selectedPuzzle === 'Sudoku'"/>
      <Hitori v-else-if="puzzleViewModel.selectedPuzzle === 'Hitori'"/>
      <EmptyPuzzle v-else-if="puzzleViewModel.selectedPuzzle === 'None'"/>
      <p v-else>Something happened</p>
    </div>
    <div class="right">
      <h1>Select a puzzle:</h1>
      <select v-model="puzzleViewModel.selectedPuzzle">
        <option v-for="puzzle in puzzles" v-bind:value="puzzle" v-bind:key="puzzle">{{ puzzle }}</option>
      </select>
      <button @click="solve()">Solve</button>
      <button @click="generate()">Generate</button>
    </div>
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
import { ICommunicatorService } from '../services/ICommunicatorService';
import { SudokuNumber } from '../types/SudokuNumber';
import { SudokuPuzzleState } from '../models/Sudoku/SudokuPuzzleState';
import { mapGetters, mapState, Store } from 'vuex';
import { RootState } from '@/store/RootState';

@Component({
  components: {
    Sudoku,
    Hitori,
    EmptyPuzzle
  },
  computed: {
    ...mapState(['puzzleViewModel', 'puzzles'])
  }
})
export default class PuzzleView extends Vue {
  @Prop() private communicatorService!: ICommunicatorService;
  @Prop() private sudokuPuzzleService!: ISudokuPuzzleSevice;

  // TODO: create a select for selecting a puzzle, on selected will show Sudoku component
  // TODO: generate button will set the puzzle state
  // TODO: solve button will solve the puzzle
  private solve(): void {
    const state: RootState = (this.$store as Store<RootState>).state;
    switch (this.getSelectedPuzzleName()) {
      case 'Sudoku': {
        if (!state.sudokuViewModel.sudokuPuzzleStateIsGenerated) {
          // TODO: show message that the sudoku puzzle needs to be generated first before it can be solved.
          return;
        }
        this.sudokuPuzzleService.solveSudoku();
        console.log('Sudoku solve called');
        break;
      }
      case 'Hitori': {
        // TODO: show message now implemented
        console.log('Hitori solve called');
        break;
      }
      case 'None': {
        // TODO: show message to select a puzzle first
        console.log('None solve called');
        break;
      }
    }
  }

  private generate(): void {
    switch (this.getSelectedPuzzleName()) {
      case 'Sudoku':
        this.sudokuPuzzleService.generateSudoku();
        console.log('Sudoku generate called');
        break;
      case 'Hitori': {
        // TODO: show message now implemented
        console.log('Hitori generate called');
        break;
      }
      case 'None': {
        // TODO: show message to select a puzzle first
        console.log('None generate called');
        break;
      }
    }
  }

  private getSelectedPuzzleName(): string {
    const state: RootState = (this.$store as Store<RootState>).state;
    return state.puzzleViewModel.selectedPuzzle;
  }
}
</script>

<style scoped>
.wrapper {
  width: 60%;
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