import { ICommunicatorService, EventHandler, MessageEventCallback } from './ICommunicatorService';
import { Message } from '@/models/Message';

export class WebSocketCommunicatorService implements ICommunicatorService {
    private identifier!: string;
    private webSocket!: WebSocket;
    private eventHandlers: EventHandler[] = [];

    constructor(webSocket: WebSocket, identifier: string) {
        this.identifier = identifier;
        this.webSocket = webSocket;
        this.webSocket.onmessage = (ev: MessageEvent) => this.messageHandler(ev);
        this.webSocket.onerror = (ev: Event) => this.errorHandler(ev);
        this.webSocket.onopen = (ev: Event) => this.openHandler(ev);
        this.webSocket.onclose = (ev: CloseEvent) => this.closeHandler(ev);
    }

    public addEventHandler(eventHandler: EventHandler): void {
        this.eventHandlers.push(eventHandler);
    }
    public removeEventHandler(eventHandler: EventHandler): void {
        this.eventHandlers = this.eventHandlers.filter((handler: EventHandler) => handler !== eventHandler);
    }
    public emit(eventName: string, data: any): void {
        this.webSocket.send(new Message(eventName, data, this.identifier).toJson());
    }

    private messageHandler(event: MessageEvent): void {
        console.log('Received message!', event);
        const message: Message = event.data as Message;
        if (message.identifier !== this.identifier) {
            return;
        }

        this.eventHandlers.forEach((eventHandler: EventHandler) => {
            if (eventHandler.eventName === message.eventName) {
                eventHandler.callback(message.data);
            }
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
