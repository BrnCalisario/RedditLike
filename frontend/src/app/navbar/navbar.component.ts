import { Component } from '@angular/core';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
    isLogged: boolean = false; 
    hrefLink: string = this.isLogged ? '/home' : '/';
}
