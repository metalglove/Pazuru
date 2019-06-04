import { ICommunicatorService, EventHandler, EventHandlerDestructor } from './ICommunicatorService';
import { Message } from '@/models/Message';

export class WebSocketCommunicatorService implements ICommunicatorService {
  private webSocket!: WebSocket;
  private eventHandlers: Map<string, EventHandler[]>;

  constructor(webSocket: WebSocket) {
    this.eventHandlers = new Map<string, EventHandler[]>();
    this.webSocket = webSocket;
    this.webSocket.onerror = (ev: Event) => this.errorHandler(ev);
    this.webSocket.onopen = (ev: Event) => this.openHandler(ev);
    this.webSocket.onclose = (ev: CloseEvent) => this.closeHandler(ev);
    this.webSocket.onmessage = (ev: MessageEvent) => this.messageHandler(ev);
  }

  public addEventHandler(eventHandler: EventHandler): void {
    if (this.eventHandlers.has(eventHandler.eventName)) {
      this.eventHandlers.get(eventHandler.eventName)!.push(eventHandler);
    } else {
      this.eventHandlers.set(eventHandler.eventName, [eventHandler]);
    }
  }
  public removeEventHandler(eventHandler: EventHandler): void {
    if (this.eventHandlers.has(eventHandler.eventName)) {
      const filteredEventHandlers: EventHandler[] = this.eventHandlers
        .get(eventHandler.eventName)!
        .filter((handler: EventHandler) => handler !== eventHandler);
      this.eventHandlers.set(eventHandler.eventName, filteredEventHandlers);
    }
  }
  public emit(eventName: string, data: any): void {
    this.webSocket.send(new Message(eventName, data).toJson());
  }
  public eventHandlerDestructor(): EventHandlerDestructor {
    return (eventHandler: EventHandler) => this.removeEventHandler(eventHandler);
  }
  private messageHandler(event: MessageEvent): void {
    // console.log('Received message!', event);
    const message: Message = JSON.parse(event.data);
    if (!this.eventHandlers.has(message.eventName)) {
      return;
    }
    if (message.success === false) {
      this.eventHandlers.get('errorHandler')!.forEach((eventHandler) => {
        eventHandler.callback(message.message);
      });
      this.eventHandlers.delete(message.eventName);
      return;
    }
    this.eventHandlers.get(message.eventName)!
      .forEach((eventHandler: EventHandler) => {
        eventHandler.callback(message.data);
    });
  }
  private errorHandler(event: Event): void {
    console.log('Error!', event);
  }
  private openHandler(event: Event): void {
    console.log('Connected!', event);
  }
  private closeHandler(event: CloseEvent): void {
    console.log('Closed!', event);
  }
}
