import { Component, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Http } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'resetPassword',
    templateUrl: './resetPassword.component.html',
    styleUrls: ['./resetPassword.component.css']
})

export class ResetPasswordComponent {
    public password: string = "";
    public c_password: string = "";
    public encrypt: string;
    private baseUrl: string;
    private http: Http;
    constructor(private auth: AuthService, http: Http, @Inject('BASE_URL') baseUrl: string, public router: Router, private activatedRoute: ActivatedRoute) {
        this.encrypt = "";
        this.activatedRoute.queryParams.subscribe(params => {
            this.encrypt = params['data'];
           
        });
        this.baseUrl = baseUrl
        this.http = http;
        
    }

    submit() {

        if (this.password != this.c_password) {
            alert("Passwords are not the same")
        }
        else if (this.password == "" || this.c_password == "") {
            alert("Password has not been enter")
        }
        else if (this.password.length < 8 || this.c_password.length < 8) {
            alert("Passwords need to be 8 characters or longer")
        }
        else if (this.encrypt == null || this.encrypt == "") {
            alert("Submission not valid, try clicking on link in email again")
        }
        else {

            var password: Password = {
                password: this.password,
                encrypt: this.encrypt
            }
            this.http.post(this.baseUrl + 'api/Auth/ResetPassword', password).subscribe(result => {
            }, error => console.error(error));

            this.router.navigateByUrl('/login');
        }



    }
}

interface Password {
    password: string,
    encrypt: string
}
