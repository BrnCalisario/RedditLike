import { AfterContentInit, Component, OnInit } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { Router } from '@angular/router';
import { User } from 'src/models/User';
import { Jwt } from 'src/DTO/Jwt';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
    constructor(private userService: UserService, private router: Router) {}

    authenticated: boolean = true;

    user: User = {
        id: 0,
        username: '',
        email: '',
        profilePicture: 0,
        groups: [],
        posts: [],
    };

    ngOnInit(): void {
        let jwt = sessionStorage.getItem('jwtSession') ?? '';

        this.userService.getUser({ Value: jwt }).subscribe({
            next: (res: User) => {
                this.user = res;

                console.log(this.user);
            },
            error: (error: any) => {
                this.router.navigate(['/']);
            },
        });
    }
}
