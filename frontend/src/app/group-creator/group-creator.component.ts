import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { User } from 'src/models/User';
import { Router } from '@angular/router';
import { Group } from 'src/models/Group';

@Component({
    selector: 'app-group-creator',
    templateUrl: './group-creator.component.html',
    styleUrls: ['./group-creator.component.css'],
})
export class GroupCreatorComponent implements OnInit {

    constructor(private userService: UserService, private router: Router) { }

    userId?: number;
    imgData?: FormData;

    groupForm: Group = {
        name: "",
        description: "",
        ownerId: 0
    }

    ngOnInit(): void {
        this.userService.validateUser()
            .subscribe(res => {
                if (!res.authenticated) {
                    this.router.navigate(['/'])
                }

                this.userId = res.userID;
                console.log(this.userId)
            })
    }

    onUpload($event: FormData) {
        this.imgData = $event
    }

    createGroup(): void {

    }
}
