import { Component, EventEmitter, Output } from '@angular/core';
import { RegisterDTO } from 'src/DTO/RegisterDTO';
import { UserService } from '../user.service';

@Component({
    selector: 'app-sign-form',
    templateUrl: './sign-form.component.html',
    styleUrls: ['./sign-form.component.css'],
})
export class SignFormComponent {
    constructor(private userService: UserService) {}

    @Output() public onChangeFormClick = new EventEmitter<any>();

    userReg: RegisterDTO = {
        username: '',
        password: '',
        email: '',
        birthdate: new Date(),
        imageData: new FormData(),
    };

    onUpload(event: FormData) {
        this.userReg.imageData = event;
    }

    onRegister() {
        this.userService.register(this.userReg).subscribe({
            next: (res) => {
                if (res.status == 200)
                    this.userReg = {
                        username: '',
                        password: '',
                        email: '',
                        birthdate: new Date(),
                        imageData: new FormData(),
                    };
            },
            error: (error) => {},
        });
    }

    onSwitch() {
        this.onChangeFormClick.emit(true);
    }
}
