import { NgZone, Injectable } from '@angular/core';
import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs/Observable";
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

declare var $: any;

export class User {
  
    public Email: string = "";
    public Name: string = "";
    public Photo: string = "";

}

export class MessageData {
    public ToUser: User;
    public FromUser: User;
    public Message: string;
    public DateCreated: Date | undefined;

    constructor(from: User, to: User) {
        this.ToUser = to;
        this.FromUser = from;
        this.Message = "";
        
    }
}

@Injectable()
export class ChatService {
    public _hubConnection: HubConnection;
    private hub: any;
    private hubName: string = 'chatMessaging';
    private connection: any;
    private messageReceivedSubject: Subject<MessageData> = new Subject<MessageData>();
    messageReceivedEvent$: Observable<MessageData>;
    constructor(private zone: NgZone) {
        this._hubConnection = new HubConnectionBuilder().withUrl('http://localhost:61209/message').build();
        this._hubConnection
            .start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));
        this.messageReceivedEvent$ = this.messageReceivedSubject.asObservable();
        //Configure Hub Connection 
        this.setupHub();
        //this.RecievedMessage();
    }
    private setupHub() {
        // create hub connection  
        //this.connection = $.hubConnection();
        // create new proxy as name already given in top  
        //this.hub = this.connection.createHubProxy(this.hubName);
    }

    private RecievedMessage(): void {
        this._hubConnection.on('onRecieved', (data: MessageData) => {
            this.messageReceivedSubject.next(data);
        });
    }

    public sendMessage(message: MessageData) {
        this._hubConnection.invoke('sendToAll', message);
    }
}