export interface ModalViewModel {
  showModal: boolean;
  body: string;
  header: string;
  footer: string;
  modalType: 'info' | 'error' | 'warning';
}
