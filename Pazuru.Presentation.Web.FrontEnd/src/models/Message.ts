export class Message {
    public identifier!: string;
    public eventName!: string;
    public data!: any;

    constructor(eventName: string, data: any, identifier: string) {
        this.identifier = identifier;
        this.eventName = eventName;
        this.data = data;
    }

    public toJson(): string {
        return JSON.stringify(this);
    }
}
