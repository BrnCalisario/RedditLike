import { AfterContentInit, Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { Router } from '@angular/router';
import { User } from 'src/models/User';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
    constructor(private userService: UserService, private router: Router) {}

    authenticated: boolean = true;

    user: User | undefined;

    ngOnInit(): void {
        this.userService.validateUser().subscribe((token) => {
            if (!token.authenticated) this.router.navigate(['/']);

            if (token.userID == 0) this.router.navigate(['/']);

            this.userService.getUser(token.userID).subscribe((user) => {
                this.user = user ?? undefined;
                console.log(user);
            });
        });
    }
}
