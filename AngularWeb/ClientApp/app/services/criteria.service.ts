import { Injectable } from '@angular/core';

@Injectable()
export class CriteriaService {
    private week: number = 1;

    constructor(
    ) { }

    setWeek(week: number) {
        this.week = week;
    }

    getWeek() {
        return this.week;
    }
}