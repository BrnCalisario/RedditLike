import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Jwt } from 'src/DTO/Jwt';
import { NavigationStart, Router } from '@angular/router';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
    constructor(private userService: UserService, private router: Router) {
        router.events.subscribe((val) => {
            if (val instanceof NavigationStart) {
                this.validateLogin();
            }
        });
    }

    ngOnInit(): void {
        this.validateLogin();
    }

    validateLogin() {
        this.userService.validateUser().subscribe((res) => {
            this.isLogged = res.authenticated;
        });
    }

    isLogged: boolean = false;
    hrefLink: string = this.isLogged ? '/home' : '/';

    logoff() {
        sessionStorage.setItem('jwtSession', '');
    }
}
