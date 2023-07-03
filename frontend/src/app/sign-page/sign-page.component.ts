import { Component } from '@angular/core';
import { RegisterDTO } from 'src/DTO/RegisterDTO';
import { UserService } from '../services/user/user.service';
import { LoginDTO } from 'src/DTO/LoginDTO';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ImageService } from '../services/image/image.service';

@Component({
    selector: 'app-sign-page',
    templateUrl: './sign-page.component.html',
    styleUrls: ['./sign-page.component.css'],
})
export class SignPageComponent {
    constructor(
        private userService: UserService,
        private router: Router,
        private imageService: ImageService
    ) {}

    isLogin: boolean = false;

    displayError: boolean = false;
    errorMessage: string = '';

    imageForm: FormData = new FormData();

    switchSign() {
        this.isLogin = !this.isLogin;
    }
}
