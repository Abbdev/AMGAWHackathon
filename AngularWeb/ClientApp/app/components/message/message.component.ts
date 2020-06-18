import { Component, Inject, OnInit, Input } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { List } from 'lodash';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';



export class MessageUser {

    public Email: string = "";
    public Name: string = "";
    public Photo: string = "";

}

export class MessageData {
    public ToUser: MessageUser;
    public FromUser: MessageUser;
    public Message: string;
    public DateCreated: Date | undefined;

    constructor(from: MessageUser, to: MessageUser) {
        this.ToUser = to;
        this.FromUser = from;
        this.Message = "";

    }
}

@Component({
    selector: 'message',
    templateUrl: './message.component.html',
    styleUrls: ['./message.component.css']
})

export class MessageComponent implements OnInit{
    public toUserName: string = "John Doe";
    @Input() fromUserData: MessageUser;
    public toUserData: MessageUser;
    public _hubConnection: HubConnection;
    public message: string = "";
    public messages: MessageData[] = [];
    public userEmail: string = "";
    private messageData: MessageData;
    public allUsers: User[] = [];
    public user: User | undefined
    constructor(public auth: AuthService, public http: Http, @Inject('BASE_URL') public baseUrl: string, public router: Router) {
        auth.keepUserLogin()
        http.get(baseUrl + 'api/Auth/GetAllUsers').subscribe(result => {
            this.allUsers = result.json() as User[];
        }, error => console.error(error));
        
        http.get(baseUrl + 'api/Auth/GetUser/' + auth.getUser()).subscribe(result => {
            this.user = result.json() as User;
        }, error => console.error(error));


        this.userEmail = auth.getUser()
        this._hubConnection = new HubConnectionBuilder().withUrl('http://localhost:61209/message').build();
        this.fromUserData = new MessageUser()
        this.fromUserData.Email = auth.getUser()
     
        this.toUserData = new MessageUser()
        this.toUserData.Name = this.toUserName
        this.toUserData.Email = "test1@gmail.com"
        this.messageData = new MessageData(this.fromUserData, this.toUserData);
    }

    ngOnInit() {
        
       
        this._hubConnection
            .start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));

      
        this._hubConnection.on('sendToAll', (data: MessageData) => {
             this.messages.push(data);
        });

    }
    sendMessage(message: string) {
        if (message != "") {
            
            this.messageData = new MessageData(this.fromUserData, this.toUserData);
            this.messageData.Message = message
            this._hubConnection.invoke('sendToAll', this.messageData);
            this.messageData.Message = ""
        }
    }

    private errorHandler(error: any, title: string) {
        console.log(title + ':' + error);
        
    }

}


interface User {
    name: string
    password: string
    email: string
    sendEmail: boolean
    image: string
    picks: List<Pick>
    isAdmin: boolean

}



interface Pick {
    year: number
    weekNum: number
    bestBet: number
    points: number
    correct: number
    total: number
    rank: number
    winners: List<number>
}
