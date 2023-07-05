import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { User } from 'src/models/User';
import { Router } from '@angular/router';

@Component({
    selector: 'app-user-config',
    templateUrl: './user-config.component.html',
    styleUrls: ['./user-config.component.css'],
})
export class UserConfigComponent implements OnInit {
    constructor(private userService: UserService, private router: Router) {}

    user: User = {
        id: 0,
        username: '',
        email: '',
        profilePicture: 0,
        groups: [],
        posts: [],
    };

    imgUrl: string = 'http://localhost:5038/img/';

    ngOnInit(): void {
        var jwt = sessionStorage.getItem('jwtSession') ?? '';
        this.userService.getUser({ Value: jwt }).subscribe({
            next: (res: User) => {
                this.user = res;
                this.imgUrl += this.user.profilePicture;
            },
            error: (error: any) => {
                this.router.navigate(['/']);
            },
        });
    }

}
