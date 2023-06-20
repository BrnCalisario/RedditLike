import { Component } from '@angular/core';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
    isLogged: boolean = true;
    hrefLink: string = this.isLogged ? '/home' : '/';
}
