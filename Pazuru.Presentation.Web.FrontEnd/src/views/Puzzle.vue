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
      <br/>
      <button @click="generate()">Generate</button>
      <br/>
      <button @click="solve()">Solve</button>
      <br/>
      <button @click="verify()">Verify</button>
      <br/>
      <button @click="describe()">Describe</button>
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

  private solve(): void {
    const state: RootState = (this.$store as Store<RootState>).state;
    switch (this.getSelectedPuzzleName()) {
      case 'Sudoku': {
        if (!state.sudokuViewModel.sudokuPuzzleStateIsGenerated) {
          state.modalViewModel.header = 'Sudoku puzzle state not found';
          state.modalViewModel.body = 'Generate a Sudoku first';
          state.modalViewModel.footer = '';
          state.modalViewModel.showModal = true;
          return;
        }
        this.sudokuPuzzleService.solveSudoku();
        break;
      }
      case 'Hitori': {
        state.modalViewModel.header = 'Invalid puzzle type';
        state.modalViewModel.body = 'Hitori is not yet implemented';
        state.modalViewModel.footer = 'Try again later';
        state.modalViewModel.showModal = true;
        break;
      }
      case 'None': {
        state.modalViewModel.header = 'Invalid solve type';
        state.modalViewModel.body = 'Select a valid solver first';
        state.modalViewModel.footer = '';
        state.modalViewModel.showModal = true;
        break;
      }
    }
  }

  private generate(): void {
    const state: RootState = (this.$store as Store<RootState>).state;
    switch (this.getSelectedPuzzleName()) {
      case 'Sudoku':
        this.sudokuPuzzleService.generateSudoku();
        break;
      case 'Hitori': {
        state.modalViewModel.header = 'Invalid puzzle type';
        state.modalViewModel.body = 'Hitori is not yet implemented';
        state.modalViewModel.footer = 'Try again later';
        state.modalViewModel.showModal = true;
        break;
      }
      case 'None': {
        state.modalViewModel.header = 'Invalid generate type';
        state.modalViewModel.body = 'Select a valid generator first';
        state.modalViewModel.footer = '';
        state.modalViewModel.showModal = true;
        break;
      }
    }
  }

  private verify(): void {
    const state: RootState = (this.$store as Store<RootState>).state;
    switch (this.getSelectedPuzzleName()) {
      case 'Sudoku': {
        if (!state.sudokuViewModel.sudokuPuzzleStateIsGenerated) {
          state.modalViewModel.header = 'Sudoku puzzle state not found';
          state.modalViewModel.body = 'Generate a Sudoku first';
          state.modalViewModel.footer = '';
          state.modalViewModel.showModal = true;
          return;
        }
        this.sudokuPuzzleService.verifySudoku();
        break;
      }
      case 'Hitori': {
        state.modalViewModel.header = 'Invalid puzzle type';
        state.modalViewModel.body = 'Hitori is not yet implemented';
        state.modalViewModel.footer = 'Try again later';
        state.modalViewModel.showModal = true;
        break;
      }
      case 'None': {
        state.modalViewModel.header = 'Invalid puzzle type';
        state.modalViewModel.body = 'Select a valid puzzle first';
        state.modalViewModel.footer = '';
        state.modalViewModel.showModal = true;
        break;
      }
    }
  }

  private describe(): void {
    const state: RootState = (this.$store as Store<RootState>).state;
    switch (this.getSelectedPuzzleName()) {
      case 'Sudoku': {
        state.modalViewModel.header = 'Sudoku';
        state.modalViewModel.body =
          'A puzzle in which missing numbers are to be filled into a 9 by 9 grid ' +
          'of squares which are subdivided into 3 by 3 boxes so that every row, every column, ' +
          'and every box contains the numbers 1 through 9.';
        state.modalViewModel.footer = 'Have fun!';
        state.modalViewModel.showModal = true;
        break;
      }
      case 'Hitori': {
        state.modalViewModel.header = 'Invalid puzzle type';
        state.modalViewModel.body = 'Hitori is not yet implemented';
        state.modalViewModel.footer = 'Try again later';
        state.modalViewModel.showModal = true;
        break;
      }
      case 'None': {
        state.modalViewModel.header = 'Invalid puzzle type';
        state.modalViewModel.body = 'Select a valid puzzle first';
        state.modalViewModel.footer = '';
        state.modalViewModel.showModal = true;
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
button, select {
  width: 70px;
}
</style>