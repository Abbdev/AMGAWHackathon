import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { AuthService } from '../../services/auth.service';
import { CriteriaService } from '../../services/criteria.service';
import { Router } from '@angular/router';

@Component({
    selector: 'rules',
    templateUrl: './rules.component.html'
})

export class RulesComponent {
    constructor(private criteria: CriteriaService, private auth: AuthService, http: Http, @Inject('BASE_URL') baseUrl: string, public router: Router) {
        auth.keepUserLogin()
    }
}