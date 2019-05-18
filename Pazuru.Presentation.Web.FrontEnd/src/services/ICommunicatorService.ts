export interface ICommunicatorService {
    addEventHandler(eventHandler: EventHandler): void;
    removeEventHandler(eventHandler: EventHandler): void;
    emit(event: string, data: any): void;
}

export type MessageEventCallback = (data: MessageEvent) => void;

// interface IEventListener {
//     (data: any): void;
// }
export class EventHandler {
    public eventName!: string;
    public callback!: MessageEventCallback;

    constructor(eventName: string, callback: MessageEventCallback) {
        this.eventName = eventName;
        this.callback = callback;
    }
}
