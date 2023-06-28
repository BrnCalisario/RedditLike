import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-landing-page',
    templateUrl: './landing-page.component.html',
    styleUrls: ['./landing-page.component.css'],
})
export class LandingPageComponent implements OnInit {
    constructor(private userService: UserService, private router: Router) {}

    ngOnInit(): void {
        this.userService.validateUser().subscribe((res) => {
            if (res.authenticated) this.router.navigate(['/home']);
        });
    }
}
