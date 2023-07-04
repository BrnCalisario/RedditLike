import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { User } from 'src/models/User';
import { Router } from '@angular/router';
import { Group } from 'src/models/Group';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { GroupService } from '../services/group/group.service';
import { ImageService } from '../services/image/image.service';

@Component({
    selector: 'app-group-creator',
    templateUrl: './group-creator.component.html',
    styleUrls: ['./group-creator.component.css'],
})
export class GroupCreatorComponent implements OnInit {
    constructor(
        private userService: UserService,
        private router: Router,
        private groupService: GroupService,
        private imageService: ImageService
    ) {}

    imgData?: FormData;

    groupForm: Group = {
        name: '',
        description: '',
        ownerId: 0,
        isMember: false,
        imageId: 0,
        userQuantity: 0,
        jwt: '',
        id: 0
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

        this.groupForm.jwt = sessionStorage.getItem("jwtSession") ?? ""
  
        this.groupService.postGroup(this.groupForm)
            .subscribe({
                next: (res: number) => {
                    
                    let groupId = res
                    console.log(groupId)

                    if(this.imgData) 
                    {
                        this.imageService.updateGroupImage(this.imgData, groupId)
                            .subscribe({
                                error: (error : HttpErrorResponse) => {
                                    console.log("erro na imagem")
                                    console.log(error)
                                }
                            })
                    }

                    this.router.navigate(["/"])
                },
                error: (error: HttpErrorResponse) => {
                    console.log("erro no grupo")
                    console.log(error)
                }
            })
    }
}
