import { Component, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { List } from 'lodash';

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css']
})

export class ProfileComponent {
    public user: User[] | undefined
    public stats: Stats[] | undefined
    public weeks: Weeks[] | undefined

    constructor(public auth: AuthService, public http: Http, @Inject('BASE_URL') public baseUrl: string, public router: Router) {
        this.auth.keepUserLogin()
           
      
        http.get(baseUrl + 'api/Auth/GetUser/' + auth.getUser()).subscribe(result => {
            this.user = result.json() as User[];
        }, error => console.error(error));

        http.get(baseUrl + 'api/Statistic/GetStatsForUser/' + auth.getUser()).subscribe(result => {
            this.stats = result.json() as Stats[];
        }, error => console.error(error));

        http.get(baseUrl + 'api/Statistic/GetProfileWeeks/' + auth.getUser()).subscribe(result => {
            this.weeks = result.json() as Weeks[];
        }, error => console.error(error));

   
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


interface Stats {
    correct: number
    percentage: number
    topRank: number
    incorrect: number
    points: number
    topPoints: number
}


interface Weeks {
    week: number
    rank: number
    correct: number
    incorrect: number
    percent: number
    bestBet: string
    points: number
}