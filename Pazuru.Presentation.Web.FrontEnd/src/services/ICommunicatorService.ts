export interface ICommunicatorService {
  addEventHandler(eventHandler: EventHandler): void;
  removeEventHandler(eventHandler: EventHandler): void;
  emit(event: string, data: any): void;
}

export type EventCallBack = (data: any) => void;
export type EventHandlerDestructor = (data: EventHandler) => void;

export abstract class EventHandler {
  public eventName!: string;
  public abstract callback: EventCallBack;

  constructor(eventName: string) {
    this.eventName = eventName;
  }
}
