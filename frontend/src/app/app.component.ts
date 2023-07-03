import { Component, DoCheck, OnInit } from '@angular/core';
import { UserService } from './services/user/user.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
})
export class AppComponent {
    authenticated: boolean = false;

    constructor(private userService: UserService) {}

    title = 'Reddit';
}
