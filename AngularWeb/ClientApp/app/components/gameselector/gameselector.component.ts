import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { CriteriaService } from '../../services/criteria.service';
import { Router } from '@angular/router';
import * as _ from 'lodash';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'gameselector',
    templateUrl: './gameselector.component.html'
})
export class GameSelectorComponent {
    public matchups: Matchup[] | undefined;
    public weeks: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
    public selectedWeek: number = 1;

    constructor(public auth: AuthService, private criteria: CriteriaService, public http: Http, @Inject('BASE_URL') public baseUrl: string, public router: Router) {
        auth.keepUserLogin()
        this.selectedWeek = this.criteria.getWeek();
        http.get(baseUrl + 'api/Games/GetMatchupsForWeek/2019/' + this.selectedWeek).subscribe(result => {
            this.matchups = result.json() as Matchup[];
        }, error => console.error(error));
    }

    UpdateSchedule() {
        this.http.post(this.baseUrl + 'api/Games/UpdateScheduleFromApi', undefined).subscribe(result => {
            this.http.get(this.baseUrl + 'api/Games/GetMatchupsForWeek/2019/' + this.selectedWeek).subscribe(result => {
                this.matchups = result.json() as Matchup[];
            }, error => console.error(error));
        }, error => console.error(error));
    }

    SaveMatchups() {
        var selectedMatchups = _.filter(this.matchups, { selected: true });
        var selectedGames = _.map(selectedMatchups, (matchup: Matchup) => {
            var result: Game = {
                away: matchup.awayTeam.school,
                awayId: matchup.awayTeamID,
                home: matchup.homeTeam.school,
                homeId: matchup.homeTeamID,
                spread: matchup.pointSpread,
                multiplier: matchup.multiplier,
                outcome: ""
            };
            return result;
        });

        var gameWeek: GameWeeks = {
            year: 2019,
            weekNum: this.selectedWeek,
            games: selectedGames,
            locked: false
        };

        this.http.post(this.baseUrl + 'api/Games/SaveMatchupsForWeek', gameWeek).subscribe(result => {
            this.http.post(this.baseUrl + 'api/Games/SendEmailForWeek', gameWeek).subscribe(result => {
            }, error => console.error(error));
        }, error => console.error(error));
    }

    SelectWeek(event: any): void {
        this.selectedWeek = event.target.value;
        this.criteria.setWeek(this.selectedWeek);
        this.http.get(this.baseUrl + 'api/Games/GetMatchupsForWeek/2019/' + this.selectedWeek).subscribe(result => {
            this.matchups = result.json() as Matchup[];
        }, error => console.error(error));
    }
}

interface Game {
    away: string,
    awayId: number,
    home: string,
    homeId: number,
    spread: number,
    multiplier: number,
    outcome: string
}

interface GameWeeks {
    year: number,
    weekNum: number,
    locked: boolean,
    games: Game[]
}

interface Matchup {
    awayTeamID: number,
    homeTeamID: number,
    awayTeam: Team,
    homeTeam: Team,
    week: number,
    season: number,
    selected: boolean,
    multiplier: number,
    pointSpread: number
}

interface Team {
    teamID: number,
    playoffRank: number,
    apRank: number,
    school: string,
    teamLogoUrl: string
}