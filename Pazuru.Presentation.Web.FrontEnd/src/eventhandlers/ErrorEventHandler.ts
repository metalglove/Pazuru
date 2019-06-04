import { EventHandler, EventCallBack, EventHandlerDestructor } from '@/services/ICommunicatorService';
import { ModalViewModel } from '@/viewmodels/ModalViewModel';

export class ErrorEventHandler extends EventHandler {
  public callback!: EventCallBack;
  private modalViewModel!: ModalViewModel;
  private eventHandlerDestructor!: EventHandlerDestructor;

  constructor(eventHandlerDestructor: EventHandlerDestructor, modalViewModel: ModalViewModel) {
    super('errorHandler');
    this.modalViewModel = modalViewModel;
    this.eventHandlerDestructor = eventHandlerDestructor;
    this.callback = (data: any) => this.showErrorMessage(data);
  }

  private showErrorMessage(data: string): void {
    console.log('Error occurred');
    this.modalViewModel.header = 'Error occurred!';
    this.modalViewModel.body = data;
    this.modalViewModel.footer = 'WOOPSIE!';
    this.modalViewModel.showModal = true;

    if (data === 'fatal') {
      this.eventHandlerDestructor(this);
    }
  }
}
