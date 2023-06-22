import { Component } from '@angular/core';
import { UserRegister } from 'src/DTO/UserRegister';


@Component({
	selector: 'app-sign-page',
	templateUrl: './sign-page.component.html',
	styleUrls: ['./sign-page.component.css']
})
export class SignPageComponent {
	isLogin : boolean = true;

	userReg : UserRegister = 
	{
		username : "",
		password : "",
		email : "",
		birthdate : new Date()
	}

	switchSign() {
		this.isLogin = !this.isLogin;
	}

	onRegister() {
		
	}
}
