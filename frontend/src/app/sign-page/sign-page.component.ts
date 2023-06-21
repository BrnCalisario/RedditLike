import { Component } from '@angular/core';

@Component({
	selector: 'app-sign-page',
	templateUrl: './sign-page.component.html',
	styleUrls: ['./sign-page.component.css']
})
export class SignPageComponent {
	isLogin : boolean = true;

	switchSign() {
		this.isLogin = !this.isLogin;
	}
}
