import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { CriteriaService } from '../../services/criteria.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'gameresults',
    templateUrl: './gameresults.component.html'
})
export class GameResultsComponent {
    public games: Game[] = [];
    public weeks: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
    public selectedWeek: number = 1;
    public locked: boolean = true;

    constructor(public auth: AuthService, private criteria: CriteriaService, public http: Http, @Inject('BASE_URL') public baseUrl: string, public router: Router) {
        auth.keepUserLogin()
        this.selectedWeek = this.criteria.getWeek();
        http.get(baseUrl + 'api/Games/GetGamesForWeek/2019/' + this.selectedWeek).subscribe(result => {
            this.games = result.json() as Game[];
        }, error => console.error(error));
        this.http.get(this.baseUrl + 'api/Games/IsWeekLocked/2019/' + this.selectedWeek).subscribe(result => {
            this.locked = result.json() as boolean;
        }, error => console.error(error));
    }

    SelectWeek(event: any): void {
        this.selectedWeek = event.target.value;
        this.criteria.setWeek(this.selectedWeek);
        this.http.get(this.baseUrl + 'api/Games/GetGamesForWeek/2019/' + this.selectedWeek).subscribe(result => {
            this.games = result.json() as Game[];
        }, error => console.error(error));
        this.http.get(this.baseUrl + 'api/Games/IsWeekLocked/2019/' + this.selectedWeek).subscribe(result => {
            this.locked = result.json() as boolean;
        }, error => console.error(error));
    }

    LockWeek(): void {
        var gameWeek: GameWeeks = {
            year: 2019,
            weekNum: this.selectedWeek,
            games: this.games,
            locked: true
        };

        this.http.post(this.baseUrl + 'api/Games/SaveMatchupsForWeek', gameWeek).subscribe(result => {
            this.http.get(this.baseUrl + 'api/Games/IsWeekLocked/2019/' + this.selectedWeek).subscribe(result => {
                this.locked = result.json() as boolean;
            }, error => console.error(error));
        }, error => console.error(error));
    }

    UpdateGames(): void {
        var gameWeek: GameWeeks = {
            year: 2019,
            weekNum: this.selectedWeek,
            games: this.games,
            locked: this.locked
        };

        this.http.post(this.baseUrl + 'api/Games/SaveMatchupsForWeek', gameWeek).subscribe(result => {
            var weekToScore: WeekToScore = {
                week: this.selectedWeek,
                year: 2019
            };

            this.http.post(this.baseUrl + 'api/Games/ScoreWeek', weekToScore).subscribe(result => {
            }, error => console.error(error));
        }, error => console.error(error));
        
    }
}

interface Game {
    away: string,
    awayTeam: Team,
    home: string,
    homeTeam: Team,
    spread: number,
    multiplier: number,
    outcome: string
}

interface Team {
    teamID: number,
    playoffRank: number,
    apRank: number,
    school: string,
    teamLogoUrl: string
}

interface GameWeeks {
    year: number,
    weekNum: number,
    locked: boolean,
    games: Game[]
}

interface WeekToScore {
    year: number,
    week: number
}