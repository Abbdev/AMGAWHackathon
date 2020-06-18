import { Component, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { StorageService } from '../../services/storage.service';
@Component({
    selector: 'registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {

    public email: string;
    public password: string;
    public c_password: string;
    public name: string;
    public sendEmail: string;
    private baseUrl: string;
    public image: string;
    private http: Http;
    public code: string;

    constructor(private auth: AuthService, http: Http, @Inject('BASE_URL') baseUrl: string, public router: Router, private session: StorageService) {
        this.email = "";
        this.password = "";
        this.c_password = "",
        this.code = "",
        this.name = "",
        this.sendEmail = "no",
        this.image = "";
        this.baseUrl = baseUrl
        this.http = http;
        this.auth.logout();
        if (sessionStorage["currentUser"] && sessionStorage["isAdmin"]) {
            session.removeItem('currentUser');
            session.removeItem('isAdmin');
        }
    }

    register() {
        if (this.image == "") {
            this.image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSEj9GWcGSZhOlf5uzGwbJkdteJjCkoEwDiPsR-vxcX5i_DmrMJzw";
        }
        if (this.name == "" || this.password == "" || this.c_password == "" || this.email == "" || this.code == "") {
            alert("Require inputs are not filled out")
        }
        else if (this.password.length < 8) {
            alert("Password needs to be 8 characters or longer");
        }
        else if (this.password != this.c_password) {
            alert("Passwords are not the same")
        }
        else if (this.code.toLowerCase() != "jointhefight2578") {
            alert("Registration code is invalid")
        }
        else {
            var RegParams = {
                name: this.name,
                email: this.email.toLowerCase(),
                password: this.password,
                confirm_pw: this.c_password,
                sendEmail: this.sendEmail,
                image: this.image
            };
            this.http.post(this.baseUrl + 'api/Registration/RegisterUser', RegParams).subscribe(result => {
                if (result.json()) {
                    this.auth.setUser(this.email, false);
                    this.router.navigateByUrl('/leaderboard');
                }
                else {
                    alert("Email already used");
                }
            }, error => console.error(error));
        }
       
    }
  
}

interface RegParams {
    email: string,
    password: string,
    name: string,
    c_password: string,
    sendEmail: string,
    image: string
}