import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { RegisterDTO } from 'src/DTO/RegisterDTO';
import { UserService } from '../user.service';
import { Router } from '@angular/router';
import { ImageService } from '../image.service';

@Component({
    selector: 'app-sign-form',
    templateUrl: './sign-form.component.html',
    styleUrls: ['./sign-form.component.css'],
})
export class SignFormComponent implements OnInit {
    constructor(
        private userService: UserService,
        private imageService: ImageService,
        private router: Router
    ) {}

    ngOnInit(): void {
        sessionStorage.setItem('jwtSession', '');
    }

    @Output() public onChangeFormClick = new EventEmitter<any>();

    userReg: RegisterDTO = {
        username: '',
        password: '',
        email: '',
        birthdate: new Date(),
    };

    imgForm?: FormData;

    onUpload(event: FormData) {
        this.imgForm = event;
    }

    onRegister() {
        let userId: number;
        this.userService.register(this.userReg).subscribe((res) => {
            userId = res;

            if (this.imgForm) {
                this.imageService
                    .updateUserAvatar(this.imgForm, userId)
                    .subscribe((res) => {});
            }

            this.router.navigate(['/sign/up']);
        });
    }

    // onRegister() {
    //     this.userService.register(this.userReg).subscribe({
    //         next: (res) => {
    //             this.userReg = {
    //                 username: '',
    //                 password: '',
    //                 email: '',
    //                 birthdate: new Date(),
    //             };

    //             this.router.navigate(['/sign/up']);
    //         },
    //         error: (error) => {
    //             console.log(error);
    //         },
    //     });
    // }
}
