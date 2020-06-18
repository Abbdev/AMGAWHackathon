import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { StorageService } from './storage.service';
@Injectable()
export class AuthService {
    private userEmail: string = "";
    private isAdmin: boolean = false;
    private session: StorageService;
    constructor(public router: Router, session: StorageService) {
        this.session = session
    }

    setUser(email: string, isAdmin: boolean) {
        this.userEmail = email;
        this.isAdmin = isAdmin;
    }

    getUser() {
        return this.userEmail;
    }

    isLoggedIn() {
        if (this.userEmail) {
            return true;
        }
        return false;
    }

    isUserAdmin() {
        return this.isAdmin;
    }

    logout() {
        this.userEmail = "";
        this.isAdmin = false;
    }

    keepUserLogin() {

        if (sessionStorage["currentUser"] && sessionStorage["isAdmin"]) {
            var currentUser = JSON.parse(this.session.getItem('currentUser') || '');
            var currentAdmin = JSON.parse(this.session.getItem('isAdmin') || '');
            if (currentUser && currentAdmin != '') {
                var isTrueSet = (currentAdmin === 'true');
                this.setUser(currentUser, isTrueSet);
            }
            else {
                this.router.navigateByUrl('/login');
            }

        }
        else {
            this.router.navigateByUrl('/login');
        }
    }
    
}