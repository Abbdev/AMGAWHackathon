import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { CriteriaService } from '../../services/criteria.service';
import { AuthService } from '../../services/auth.service';
import * as _ from 'lodash';
import { Router } from '@angular/router';

@Component({
    selector: 'leaderboard',
    templateUrl: './leaderboard.component.html'
})
export class LeaderboardComponent {
    public leaderboard: LeaderboardRow[] | undefined;
    public weeks: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
    public selectedWeek: number = 1;
    public userEmail: string = "";

    constructor(public auth: AuthService, private criteria: CriteriaService, public http: Http, @Inject('BASE_URL') public baseUrl: string, public router: Router) {
        auth.keepUserLogin()
        this.selectedWeek = this.criteria.getWeek();
        this.userEmail = auth.getUser();
        http.get(baseUrl + 'api/Leaderboard/GetLeaderboardForWeek/2019/' + this.selectedWeek).subscribe(result => {
            this.leaderboard = result.json() as LeaderboardRow[];
        }, error => console.error(error));
    }

    SelectWeek(event: any): void {
        this.selectedWeek = event.target.value;
        this.criteria.setWeek(this.selectedWeek);
        this.http.get(this.baseUrl + 'api/Leaderboard/GetLeaderboardForWeek/2019/' + this.selectedWeek).subscribe(result => {
            this.leaderboard = result.json() as LeaderboardRow[];
        }, error => console.error(error));
    }
}

interface LeaderboardRow {
    rank: number,
    previous: number,
    name: string,
    image: string,
    email: string,
    correct: number,
    incorrect: number,
    percent: number,
    lastWeek: string,
    bestBet: string,
    points: number
}