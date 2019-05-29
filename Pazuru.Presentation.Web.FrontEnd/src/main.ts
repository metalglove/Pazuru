import Vue, { Component } from 'vue';
import App from './App.vue';
import Home from '@/views/Home.vue';
import Router from 'vue-router';
import Vuex, { Store } from 'vuex';
import { ICommunicatorService } from './services/ICommunicatorService';
import { WebSocketCommunicatorService } from './services/WebSocketCommunicatorService';
import { PuzzleViewModel } from './viewmodels/PuzzleViewModel';
import { RootState } from './store/RootState';
import { ISudokuPuzzleSevice } from './services/ISudokuPuzzleService';
import { SudokuPuzzleService } from './services/SudokuPuzzleService';
import { SudokuViewModel, Cell } from './viewmodels/SudokuViewModel';
import { SudokuUtilities } from './utilities/SudokuUtilities';
import { PreviouslySolvedPuzzlesViewModel } from './viewmodels/PreviouslySolvedPuzzlesViewModel';
import { IPuzzleService } from './services/IPuzzleService';
import { PuzzleService } from './services/PuzzleService';

// Vue config
Vue.config.productionTip = false;
Vue.use(Router);
Vue.use(Vuex);

// Create state dependencies
const puzzleViewModel: PuzzleViewModel = { selectedPuzzle: 'None' };
const sudokuViewModel: SudokuViewModel = {
  moves: [],
  sudokuPuzzleState: undefined,
  sudokuPuzzleStateIsGenerated: false
};
const previouslySolvedPuzzlesViewModel: PreviouslySolvedPuzzlesViewModel = {
  previouslySolvedPuzzles: [],
  selectedPuzzle: 'None'
};

// Create the store
const store: Store<RootState> = new Store<RootState>({
  state: {
    puzzleViewModel,
    sudokuViewModel,
    sudokuPuzzleLength: 9,
    puzzles: ['Sudoku', 'Hitori', 'None'],
    previouslySolvedPuzzlesViewModel
  },
  getters: {
    getSudokuPuzzleLength: (state: RootState): number => {
      return state.sudokuPuzzleLength;
    },
    getSudokuCell: (state: RootState) => (row: number, column: number): Cell => {
      return SudokuUtilities.getCellAtIndex(state.sudokuViewModel.sudokuPuzzleState!, row, column);
    }
  },
  mutations: {

  },
  actions: {

  },
});

// Create the dependencies for the root component
const webSocket: WebSocket = new WebSocket('wss://localhost:44399/puzzle');
const communicatorService: ICommunicatorService = new WebSocketCommunicatorService(webSocket);
const sudokuPuzzleService: ISudokuPuzzleSevice = new SudokuPuzzleService(communicatorService, store.state);
const puzzleService: IPuzzleService = new PuzzleService(communicatorService, store.state);

/* tslint:disable */
const loadView = (name: string): any => import(`./views/${name}.vue`);
/* tslint:enable */

// Create the router
const router: Router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home,
    },
    {
      path: '/puzzle',
      name: 'Puzzle',
      component: () => loadView('Puzzle'),
      props: {
        communicatorService,
        sudokuPuzzleService
      }
    },
    {
      path: '/previouslysolvedpuzzles',
      name: 'Previously Solved Puzzles',
      component: () => loadView('PreviouslySolvedPuzzles'),
      props: {
        communicatorService,
        puzzleService
      }
    }
  ]
});

const app: Vue = new Vue({
  router,
  store,
  render: (h) => h(App),
});

app.$mount('#app');
