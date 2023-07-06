import { Component, OnInit } from '@angular/core';
import { Group } from 'src/models/Group';
import { HttpErrorResponse } from '@angular/common/http';
import { GroupService } from '../services/group/group.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-group-search',
    templateUrl: './group-search.component.html',
    styleUrls: ['./group-search.component.css'],
})
export class GroupSearchComponent implements OnInit {
    constructor(private groupService: GroupService, private router: Router) {}

    groupList: Group[] = [];

    getImgUrl = (id: number | null): string => {
        if (id == 0 || id === undefined || id == null)
            return '../../assets/image/avatar-placeholder.png';

        return 'http://localhost:5038/img/' + id;
    };

    ngOnInit(): void {
        let jwt = sessionStorage.getItem('jwtSession') ?? '';

        this.groupService.listGroups({ Value: jwt }).subscribe({
            next: (res: Group[]) => {
                console.log(res);
                this.groupList = res;
            },
            error: (error: HttpErrorResponse) => {
                console.log(error);
            },
        });
    }

    enterGroup(group: Group): void {
        let jwt = sessionStorage.getItem('jwtSession') ?? '';

        console.log(group.id);

        this.groupService
            .enterGroup({ jwt: jwt, groupId: group.id })
            .subscribe((res) => {
                console.log(res);

                let groupUrl = '/group/' + group.name + '/feed';

                this.router.navigate([groupUrl]);
            });
    }
}
