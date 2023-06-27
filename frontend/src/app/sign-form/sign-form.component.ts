import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { RegisterDTO } from 'src/DTO/RegisterDTO';
import { UserService } from '../user.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-sign-form',
    templateUrl: './sign-form.component.html',
    styleUrls: ['./sign-form.component.css'],
})
export class SignFormComponent implements OnInit {
    constructor(private userService: UserService, private router: Router) {}
    
    ngOnInit(): void {
        sessionStorage.setItem("jwtSession", '')
    }

    @Output() public onChangeFormClick = new EventEmitter<any>();

    userReg: RegisterDTO = {
        username: '',
        password: '',
        email: '',
        birthdate: new Date(),
    };

    onRegister() {
        this.userService.register(this.userReg).subscribe({
            next: (res) => {
                if (res.status == 200)
                    this.userReg = {
                        username: '',
                        password: '',
                        email: '',
                        birthdate: new Date(),
                    };

                    this.router.navigate(["/sign/up"])
            },
            error: (error) => {
                console.log(error)
            },
        });
    }
}
