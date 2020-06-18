import { Component, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { StorageService } from '../../services/storage.service';
@Component({
    selector: 'forgotPassword',
    templateUrl: './forgotPassword.component.html',
    styleUrls: ['./forgotPassword.component.css']
})

export class ForgotPasswordComponent {
    public email: string = "";
    private baseUrl: string;
    private http: Http;
    constructor(private auth: AuthService, http: Http, @Inject('BASE_URL') baseUrl: string, public router: Router, private session: StorageService) {
        this.email = "";
        this.baseUrl = baseUrl
        this.http = http;
        if (sessionStorage["currentUser"] && sessionStorage["isAdmin"]) {
            session.removeItem('currentUser');
            session.removeItem('isAdmin');
        }
    }

    submit(){
        var email: Email = {
            email: this.email.toLowerCase()
        }


        this.http.post(this.baseUrl + 'api/Auth/ForgotPasswordEmail', email).subscribe(result => {
            if (result.json()) {
                this.router.navigateByUrl('/login');
            }
            else {
                alert("User with email does not exist");
            }
        }, error => console.error(error));

        
    }
}

interface Email {
    email: string
}
