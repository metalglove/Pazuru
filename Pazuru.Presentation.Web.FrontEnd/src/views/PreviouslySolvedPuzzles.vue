<template>
  <div class="wrapper">
    <div class="left">
      <div v-for="(value) in previouslySolvedPuzzles" v-bind:key="value">
        {{ name }}
      </div>
    </div>
    <div class="right">
      <p>
        right
      </p>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ISudokuPuzzleSevice } from '../services/ISudokuPuzzleService';
import { ICommunicatorService } from '../services/ICommunicatorService';
import { SudokuPuzzleState } from '../models/Sudoku/SudokuPuzzleState';
import { mapGetters, mapState, Store } from 'vuex';
import { RootState } from '@/store/RootState';

@Component({
  computed: {
    ...mapState(['previouslySolvedPuzzles'])
  }
})
export default class PreviouslySolvedPuzzlesView extends Vue {
  @Prop() private communicatorService!: ICommunicatorService;
  @Prop() private sudokuPuzzleService!: ISudokuPuzzleSevice;

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