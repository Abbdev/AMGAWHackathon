import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import * as _ from 'lodash';
import { List } from 'lodash';
import { AuthService } from '../../services/auth.service';
import { CriteriaService } from '../../services/criteria.service';
import { Router } from '@angular/router';

@Component({
    selector: 'userPicks',
    templateUrl: './userPicks.component.html'
})
export class UserPicksComponent {
    public games: Game[] = [];
    public pick: Pick = {
        year: 0,
        weekNum: 0,
        bestBet: 0,
        points: 0,
        correct: 0,
        total: 0,
        rank: 0,
        winners: [],
        email: ""
    }
    public weeks: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
    private baseUrl: string;
    private http: Http;
    public selectedWeek: number = 1;
    public selectedYear: number = 2019;
    public email: string = "";
    public bestBet: number = 0;
    public locked: boolean = true;

    constructor(private criteria: CriteriaService, private auth: AuthService, http: Http, @Inject('BASE_URL') baseUrl: string, public router: Router) {
        auth.keepUserLogin()
        this.baseUrl = baseUrl
        this.http = http;
        this.email = this.auth.getUser()
        this.selectedWeek = this.criteria.getWeek();
        http.get(baseUrl + 'api/Games/GetGamesForWeek/2019/' + this.selectedWeek).subscribe(result => {
            this.games = result.json() as Game[];
            http.get(baseUrl + 'api/Games/GetPickForWeek/' + this.email + '/' + this.selectedWeek + '/' + this.selectedYear).subscribe(result => {
                this.pick = result.json() as Pick;
                if (!this.pick) {
                    this.pick = {
                        year: this.selectedYear,
                        weekNum: this.selectedWeek,
                        bestBet: 0,
                        points: 0,
                        correct: 0,
                        total: 0,
                        rank: 0,
                        winners: [],
                        email: this.email
                    }
                }
                this.PreselectGames();
            }, error => console.error(error));
        }, error => console.error(error));
        http.get(baseUrl + 'api/Games/IsWeekLocked/' + this.selectedYear + '/' + this.selectedWeek).subscribe(result => {
            this.locked = result.json() as boolean;
        }, error => console.error(error));
        
    }

    PreselectGames() {
        _.forEach(this.games, (game: Game) => {
            if (_.includes(this.pick.winners, game.awayId)) {
                game.selected = game.awayId;
            }
            if (_.includes(this.pick.winners, game.homeId)) {
                game.selected = game.homeId;
            }
        });
    }

    SelectWeek(event: any): void {
        this.selectedWeek = event.target.value;
        this.criteria.setWeek(this.selectedWeek);
        this.http.get(this.baseUrl + 'api/Games/GetGamesForWeek/2019/' + this.selectedWeek).subscribe(result => {
            this.games = result.json() as Game[];
            this.http.get(this.baseUrl + 'api/Games/GetPickForWeek/' + this.email + '/' + this.selectedWeek + '/' + this.selectedYear).subscribe(result => {
                this.pick = result.json() as Pick;
                if (!this.pick) {
                    this.pick = {
                        year: this.selectedYear,
                        weekNum: this.selectedWeek,
                        bestBet: 0,
                        points: 0,
                        correct: 0,
                        total: 0,
                        rank: 0,
                        winners: [],
                        email: this.email
                    }
                }
                this.PreselectGames();
            }, error => console.error(error));
        }, error => console.error(error));
        this.http.get(this.baseUrl + 'api/Games/IsWeekLocked/' + this.selectedYear + '/' + this.selectedWeek).subscribe(result => {
            this.locked = result.json() as boolean;
        }, error => console.error(error));
    }

    SaveGames() {

        var winners = _.map(this.games, (game: Game) => {
            return game.selected;
        });
        var flag = false;

        for (var i = 0, l = winners.length; i < l; i++) {
            if (typeof (winners[i]) == 'undefined') {
                flag = true;
            };
        };

        if (flag) {
            alert("Missing winner pick(s)")
        }
        else {
    
            var pick: Pick = {
                year: 2019,
                weekNum: this.selectedWeek,
                bestBet: this.pick.bestBet,
                points: this.pick.points,
                correct: this.pick.correct,
                total: this.pick.total,
                rank: this.pick.rank,
                winners: winners,
                email: this.email

            };



            var userPick: UserPick = {
                pick: pick,
                email: this.email
            }

            this.http.post(this.baseUrl + 'api/Games/SaveGamePicksForWeek', userPick).subscribe(result => {
            }, error => console.error(error));
        }

       
    }

    SelectBestBet(event: any) {
        this.pick.bestBet = event.target.value;
    }

  
    
}

interface Game {
    away: string,
    awayId: number,
    home: string,
    homeId: number,
    spread: number,
    multiplier: number,
    outcome: string,
    selected: number
}

interface Pick {
    year: number,
    weekNum: number,
    bestBet: number,
    points: number,
    correct: number,
    total: number,
    rank: number,
    winners: List<number>,
    email: string | undefined
}

interface UserPick {
    email: string,
    pick: Pick
}