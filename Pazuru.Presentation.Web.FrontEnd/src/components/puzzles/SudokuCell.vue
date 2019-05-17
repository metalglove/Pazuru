<template>
    <td v-bind:row="sudokuCell.row" v-bind:column="sudokuCell.column">
        {{ sudokuCell.number }}
    </td>
</template>
<!-- 
    :class="['i'+row, 'j'+column, cell.editable ? 'editable': 'not-editable']" 
 <input type="tel" v-model="cell.number" :readonly="!cell.editable"
        :data-row="row" :data-column="column" /> 
                    <!-- @click="cellClick"
    @keyup="cellKeyUp" -->
<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Cell } from '@/viewmodels/SudokuViewModel';

@Component({})
export default class Sudoku extends Vue {
    @Prop() private sudokuCell!: Cell;
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

.i2, .i5 {
  border-bottom: 3px solid black !important;
}

.j2, .j5 {
  border-right: 3px solid black !important;
}

.not-editable input {
  font-weight: bold;
}

.has-conflict input {
  color: red;
}
</style>
