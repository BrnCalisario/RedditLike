import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-user-card',
    templateUrl: './user-card.component.html',
    styleUrls: ['./user-card.component.css'],
})
export class UserCardComponent {
    @Input() userName: string = '';
    @Input() profilePictureID: number = 0;

    profilePic = () => {
        if (this.profilePictureID == 0)
            return '../assets/image/avatar-placeholder.png';
        else return 'http://localhost:5038/img/' + this.profilePictureID;
    };

    logoff() {
        sessionStorage.setItem('jwtSession', '');
    }
}
