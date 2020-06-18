import { Component, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { StorageService } from '../../services/storage.service';
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {

    public email: string;
    public password: string;
    private baseUrl: string;
    private http: Http;
    private session: StorageService;
    constructor(private auth: AuthService, http: Http, @Inject('BASE_URL') baseUrl: string, public router: Router, session: StorageService,
                @Inject(PLATFORM_ID) platformId: Object) {
        this.email = "";
        this.password = "";
        this.auth.logout();
        this.baseUrl = baseUrl
        this.http = http;
        this.session = session;
        if (isPlatformBrowser(platformId)) {
            if (session.checkKey('currentUser') && session.checkKey('isAdmin')) {
                session.removeItem('currentUser');
                session.removeItem('isAdmin');
            }
        }
       
       
    }

    login() {
        this.email = this.email.toLowerCase();
        var loginParams = { email: this.email, password: this.password };

        this.http.post(this.baseUrl + 'api/Auth/LoginUser', loginParams).subscribe(result => {
            //this.leaderboard = result.json() as LeaderboardRow[];
            var loginResult: LoginReturn = result.json() as LoginReturn;
            if (loginResult.loggedIn) {
                this.session.setItem('currentUser', JSON.stringify(this.email));
                this.session.setItem('isAdmin', JSON.stringify(loginResult.isAdmin.toString()));
                this.auth.setUser(this.email, loginResult.isAdmin);
                this.router.navigateByUrl('/leaderboard');
            }
            else {
                alert("Invalid Username or Password");
            }
        }, error => console.error(error));
        
    }
}

interface LoginParams {
    email: string,
    password: string
}

interface LoginReturn {
    loggedIn: boolean,
    isAdmin: boolean
}