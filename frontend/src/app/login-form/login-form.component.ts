import { Component, EventEmitter, Output } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { LoginDTO } from 'src/DTO/LoginDTO';
import { Router } from '@angular/router';
import { HttpResponse } from '@angular/common/http';

@Component({
    selector: 'app-login-form',
    templateUrl: './login-form.component.html',
    styleUrls: ['./login-form.component.css'],
})
export class LoginFormComponent {
    @Output() public onChangeFormClick = new EventEmitter<any>();

    constructor(private userService: UserService, private router: Router) {}

    userLogin: LoginDTO = {
        email: '',
        password: '',
    };

    loginFailure: boolean = false;

    onLogin() {
        this.userService.login(this.userLogin).subscribe((res) => {
            if (!res.success) {
                this.loginFailure = true;
                return;
            }

            sessionStorage.setItem('jwtSession', res.jwt);
            this.router.navigate(['/home']);
        });
    }

    onSwitch() {
        this.onChangeFormClick.emit(true);
    }
}
