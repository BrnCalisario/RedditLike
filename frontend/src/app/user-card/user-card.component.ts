import { Component, Input, OnInit } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { User } from 'src/models/User';

@Component({
    selector: 'app-user-card',
    templateUrl: './user-card.component.html',
    styleUrls: ['./user-card.component.css'],
})
export class UserCardComponent implements OnInit {
    constructor(private userService: UserService) {}

    @Input() isGroup: boolean = false;
    @Input() roleName: string = '';

    userName: string = '';
    profilePictureID?: number | null = 0;

    ngOnInit(): void {
        let jwt = sessionStorage.getItem('jwtSession') ?? '';

        this.userService.getUser({ Value: jwt }).subscribe({
            next: (res: User) => {
                this.userName = res.username;
                this.profilePictureID = res.profilePicture;
            },
        });
    }

    profilePic = () => {
        if (this.profilePictureID == 0 || this.profilePictureID === null)
            return '../assets/image/avatar-placeholder.png';
        else return 'http://localhost:5038/img/' + this.profilePictureID;
    };

    logoff() {
        sessionStorage.setItem('jwtSession', '');
    }
}
