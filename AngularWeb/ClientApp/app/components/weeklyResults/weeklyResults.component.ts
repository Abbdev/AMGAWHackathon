import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { CriteriaService } from '../../services/criteria.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'weeklyResults',
    templateUrl: './weeklyResults.component.html'
})
export class WeeklyResultsComponent {
    public resultResponse: ResultsResponse | undefined;
    public weeks: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
    public selectedWeek: number = 1;
    public userEmail: string = "";

    constructor(private auth: AuthService, private criteria: CriteriaService, public http: Http, @Inject('BASE_URL') public baseUrl: string, public router: Router) {
        auth.keepUserLogin()
        this.selectedWeek = this.criteria.getWeek();
        this.userEmail = auth.getUser();
        this.http.get(this.baseUrl + 'api/Games/IsWeekLocked/2019/' + this.selectedWeek).subscribe(result => {
            if (result.json() as boolean) {
                http.get(baseUrl + 'api/Leaderboard/GetResultsForWeek/2019/' + this.selectedWeek).subscribe(result => {
                    this.resultResponse = result.json() as ResultsResponse;
                }, error => console.error(error));
            }
            else {
                this.resultResponse = undefined;
            }
        }, error => console.error(error));
    }

    SelectWeek(event: any): void {
        this.selectedWeek = event.target.value;
        this.criteria.setWeek(this.selectedWeek);
        this.http.get(this.baseUrl + 'api/Games/IsWeekLocked/2019/' + this.selectedWeek).subscribe(result => {
            if (result.json() as boolean) {
                this.http.get(this.baseUrl + 'api/Leaderboard/GetResultsForWeek/2019/' + this.selectedWeek).subscribe(result => {
                    this.resultResponse = result.json() as ResultsResponse;
                }, error => console.error(error));
            }
            else {
                this.resultResponse = undefined;
            }
        }, error => console.error(error));
    }
}

interface Team {
    teamID: number,
    playoffRank: number,
    apRank: number,
    school: string,
    teamLogoUrl: string,
    outcome: string
}

interface User {
    name: string,
    image: string,
    email: string
}

interface ResultsRow {
    user: User,
    teams: Team[],
    bestBetId: number,
    rank: number
}

interface Game {
    away: string,
    awayTeam: Team,
    home: string,
    homeTeam: Team,
    spread: number,
    multiplier: number,
    outcome: string,
    dateTime: string
}

interface ResultsResponse {
    games: Game[],
    rows: ResultsRow[]
}