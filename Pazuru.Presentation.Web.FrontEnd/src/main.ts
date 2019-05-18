import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
// import io from 'socket.io-client';
import { ICommunicatorService } from './services/ICommunicatorService';
import { WebSocketCommunicatorService } from './services/WebSocketCommunicatorService';

// import { SocketIOCommunicatorService } from './services/SocketIOCommunicatorService';
// const socket: SocketIOClient.Socket = io.connect('https://localhost:44399'); // .connect('http://localhost:9999');
// const communicatorService: ICommunicatorService = new SocketIOCommunicatorService(socket);
const webSocket: WebSocket = new WebSocket('wss://echo.websocket.org', ['json']);
const communicatorService: ICommunicatorService = new WebSocketCommunicatorService(webSocket, 'mario');

Vue.config.productionTip = false;
new Vue({
  router,
  store,
  data: {
    communicatorService
  },
  render: (h) => h(App),
}).$mount('#app');
