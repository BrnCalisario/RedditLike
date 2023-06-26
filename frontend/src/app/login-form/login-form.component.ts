import { Component, EventEmitter, Output } from '@angular/core';
import { UserService } from '../user.service';
import { LoginDTO } from 'src/DTO/LoginDTO';

@Component({
    selector: 'app-login-form',
    templateUrl: './login-form.component.html',
    styleUrls: ['./login-form.component.css'],
})
export class LoginFormComponent {
    @Output() public onChangeFormClick = new EventEmitter<any>();

    constructor(private userService: UserService) {}

    userLogin: LoginDTO = {
        email: '',
        password: '',
    };

    onLogin() {
        // this.userService.login(this.userLogin).subscribe({
        //     next: (res) => {
        //         if (res.status == 200) this.router.navigate(['/home']);
        //     },
        //     error: (error) => {
        //         if (error.status == 400) {
        //             this.displayError = true;
        //             this.errorMessage = 'Login ou senha inválidos';
        //         }
        //         if (error.status == 500) {
        //             this.displayError = true;
        //             this.errorMessage = 'Erro interno no servidor';
        //         }
        //     },
        // });
    }

    onSwitch() {
        this.onChangeFormClick.emit(true);
    }
}
