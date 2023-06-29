import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { User } from 'src/models/User';
import { Router } from '@angular/router';
import { Group } from 'src/models/Group';
import { GroupService } from '../services/group.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';

@Component({
    selector: 'app-group-creator',
    templateUrl: './group-creator.component.html',
    styleUrls: ['./group-creator.component.css'],
})
export class GroupCreatorComponent implements OnInit {
    constructor(
        private userService: UserService,
        private router: Router,
        private groupService: GroupService
    ) {}

    imgData?: FormData;

    groupForm: Group = {
        name: '',
        description: '',
        ownerId: 0,
        userParticipates: false,
        imageId: 0,
        userQuantity: 0,
    };

    ngOnInit(): void {
        this.userService.validateUser().subscribe((res) => {
            if (!res.authenticated) {
                this.router.navigate(['/']);
            }
            this.groupForm.ownerId = res.userID;
        });
    }

    onUpload($event: FormData) {
        this.imgData = $event;
    }

    createGroup(): void {
        // this.groupService.postGroup(this.groupForm)
        //     .subscribe((res) => {
                
        //         this.groupService.updateGroupImage()
        //         // this.router.navigate(['/home']);
        //     });

        this.groupService.postGroup(this.groupForm)
            .subscribe({
                next: (res: number) => {
                    
                    console.log(res)

                    if(this.imgData) 
                    {
                        this.groupService.updateGroupImage(this.imgData, res)
                            .subscribe({
                                error: (error : HttpErrorResponse) => {
                                    console.log(error)
                                }
                            })
                    }

                    this.router.navigate(["/"])
                },
                error: (error: HttpErrorResponse) => {
                    console.log(error)
                }
            })
    }
}
