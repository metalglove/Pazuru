import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
// import { ICommunicatorService } from './services/ICommunicatorService';
// import { WebSocketCommunicatorService } from './services/WebSocketCommunicatorService';

// // const webSocket: WebSocket = new WebSocket('wss://localhost:44399/api/puzzle'); 
// const webSocket: WebSocket = new WebSocket('wss://localhost:44399/puzzle');
// // const webSocket: WebSocket = new WebSocket('ws://localhost:51747/puzzle'); 
// // const webSocket: WebSocket = new WebSocket('ws://localhost:51747/api/puzzle');

// const communicatorService: ICommunicatorService = new WebSocketCommunicatorService(webSocket);
Vue.config.productionTip = false;
new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
