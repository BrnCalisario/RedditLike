import { Component } from '@angular/core';
import { RegisterDTO } from 'src/DTO/RegisterDTO';
import { UserService } from '../user.service';
import { LoginDTO } from 'src/DTO/LoginDTO';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
    selector: 'app-sign-page',
    templateUrl: './sign-page.component.html',
    styleUrls: ['./sign-page.component.css'],
})
export class SignPageComponent {
    constructor(private userService: UserService, private router: Router) { }

    isLogin: boolean = true;

    displayError: boolean = false;
    errorMessage: string = "";

    userReg: RegisterDTO = {
        username: '',
        password: '',
        email: '',
        birthdate: new Date(),
    };

    userLogin: LoginDTO = {
        email: '',
        password: '',
    };

    switchSign() {
        this.isLogin = !this.isLogin;
    }

    onRegister() { 
        this.userService.register(this.userReg)
            .subscribe({
                next: (res) => {
                    if(res.status == 200)
                        this.userReg = {
                            username: '',
                            password: '',
                            email: '',
                            birthdate: new Date(),
                        };
                        
                        this.isLogin = true;
                },
                error: (error) => {
                    
                }
            })
    }

    onLogin() {
        this.userService.login(this.userLogin)
            .subscribe({
                next: (res) => {
                    if(res.status == 200)
                        this.router.navigate(["/home"])
                },
                error: (error) => {
                    if(error.status == 400) {
                        this.displayError = true;
                        this.errorMessage = "Login ou senha inválidos"
                    }
                    if(error.status == 500) {
                        this.displayError = true;
                        this.errorMessage = "Erro interno no servidor"
                    }
                }
            })

    }
}
