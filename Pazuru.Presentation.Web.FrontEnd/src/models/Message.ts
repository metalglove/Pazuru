export class Message {
  public eventName!: string;
  public data!: any;
  public message!: string;
  public success!: boolean;

  constructor(eventName: string, data: any) {
    this.eventName = eventName;
    this.data = data;
  }
  public toJson(): string {
    return JSON.stringify(this, null, '');
  }
}
