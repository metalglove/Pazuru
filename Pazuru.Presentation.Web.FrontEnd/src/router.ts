import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import { ICommunicatorService } from './services/ICommunicatorService';
import { WebSocketCommunicatorService } from './services/WebSocketCommunicatorService';

Vue.use(Router);
// const webSocket: WebSocket = new WebSocket('wss://localhost:44399/api/puzzle'); 
const webSocket: WebSocket = new WebSocket('wss://localhost:44399/puzzle');
// const webSocket: WebSocket = new WebSocket('ws://localhost:51747/puzzle'); 
// const webSocket: WebSocket = new WebSocket('ws://localhost:51747/api/puzzle');

const communicatorService: ICommunicatorService = new WebSocketCommunicatorService(webSocket);
export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import(/* webpackChunkName: "about" */ './views/About.vue'),
    },
    {
      path: '/puzzle',
      name: 'puzzle',
      component: () => import('./views/Puzzle.vue'),
      props: {
        communicatorService
      }
    },
  ],
});
