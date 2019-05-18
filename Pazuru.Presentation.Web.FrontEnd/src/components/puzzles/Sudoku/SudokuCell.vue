<template>
    <td :class="getClass()">
      <input type="number" 
             v-model.number="sudokuCell.number" 
             :readonly="!sudokuCell.editable" /> 
    </td>
</template>
<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Cell } from '@/viewmodels/SudokuViewModel';

@Component({})
export default class Sudoku extends Vue {
    @Prop() private sudokuCell!: Cell;

    private getClass(): string[] {
      return [
        'row' + this.sudokuCell.row,
        'column' + this.sudokuCell.column,
        this.sudokuCell.editable ? 'editable' : 'not-editable',
        this.isDarkCell() ? 'darkcell' : ''
        ];
    }

    private isDarkCell(): boolean {
      if (this.sudokuCell.column >= 3 && this.sudokuCell.column <= 5 &&
          (this.sudokuCell.row < 3 || this.sudokuCell.row > 5)) {
          return true;
      }
      if (this.sudokuCell.row >= 3 && this.sudokuCell.row <= 5 &&
          (this.sudokuCell.column < 3 || this.sudokuCell.column > 5)) {
          return true;
      }
      return false;
    }
}
</script>

<style scoped>
td {
  border: 1px solid black;
  width: 35px;
  height: 35px;
}

input {
  width: 35px;
  height: 35px;
  border: 0px;
  text-align: center;
}
input::-webkit-inner-spin-button, 
input::-webkit-outer-spin-button {
  -webkit-appearance: none; 
  margin: 0; 
}
.row2, .row5 {
  border-bottom: 3px solid black !important;
}
.column2, .column5 {
  border-right: 3px solid black !important;
}
.darkcell > input {
  background-color: lightgrey;
}
.not-editable input {
  font-weight: bold;
}
</style>
