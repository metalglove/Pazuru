import { expect } from 'chai';
import { shallowMount } from '@vue/test-utils';
import Puzzle from '@/views/Puzzle.vue';
import { ICommunicatorService } from '@/services/ICommunicatorService';
import { WebSocketCommunicatorService } from '@/services/WebSocketCommunicatorService';
import { ErrorEventHandler } from '@/eventhandlers/ErrorEventHandler';
import { ISudokuPuzzleSevice } from '@/services/ISudokuPuzzleService';
import { SudokuPuzzleService } from '@/services/SudokuPuzzleService';

describe('Puzzle.vue', () => {
  it('selects sudoku', () => {
    // const communicatorService: ICommunicatorService = null;
    // communicatorService.addEventHandler(
    //   new ErrorEventHandler(
    //     communicatorService.eventHandlerDestructor(),
    //     store.state.modalViewModel));
    // const sudokuPuzzleService: ISudokuPuzzleSevice =
    //   new SudokuPuzzleService(communicatorService, store.state);
    // propsData: {
    //   sudokuPuzzleService
    // }
    const wrapper = shallowMount(Puzzle);
    const selectContainsSudoku: boolean = wrapper.find('select').contains('Sudoku');
    expect(selectContainsSudoku).equal(true);
    // expect(wrapper.)
  });
});
