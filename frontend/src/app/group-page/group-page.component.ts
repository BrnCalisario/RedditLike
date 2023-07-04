import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Group } from 'src/models/Group';
import { GroupService } from '../services/group/group.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-group-page',
    templateUrl: './group-page.component.html',
    styleUrls: ['./group-page.component.css'],
})
export class GroupPageComponent {
    constructor(
        private route: ActivatedRoute, 
        private router: Router,
        private groupService: GroupService) {}

    group : Group = {
        name: '',
        description: '',
        ownerId: 0,
        isMember: false,
        imageId: null,
        userQuantity: 0,
        jwt: ""
    }

    subscription: any;

    ngOnInit() {
        this.subscription = this.route.params.subscribe((params) => {
            let groupName = params['name']
            let jwt = sessionStorage.getItem("jwtSession") ?? ""

            this.groupService.getGroup({ jwt: jwt, name : groupName })
                .subscribe({
                    next: (group: Group) => {
                        this.group = group
                    },
                    error: (error: HttpErrorResponse) => {
                        this.router.navigate(["/404"])
                    }
                })

        });
    }

    // ngOnDestroy() {
    //     this.subscription.unsubscribe();
    // }

    // @Input() group: Group = {
    //     name: 'Gatinhos',
    //     description: 'Grupo sobre gatinhos',
    // };
}
