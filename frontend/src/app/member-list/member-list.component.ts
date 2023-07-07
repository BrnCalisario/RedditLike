import { Component, OnInit } from '@angular/core';
import { MemberItem } from 'src/DTO/MemberDTO/MemberDTO';
import { GroupService } from '../services/group/group.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-member-list',
    templateUrl: './member-list.component.html',
    styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
    constructor(private groupService: GroupService, private router: Router) {}

    groupId: number = 0;
    jwt = sessionStorage.getItem('jwtSession') ?? '';

    ngOnInit(): void {
        let groupName = this.router.url.split('/')[2];

        this.groupService
            .getGroup({ jwt: this.jwt, name: groupName })
            .subscribe((res) => {
                this.groupId = res.id;

                this.groupService
                    .listMembers({ jwt: this.jwt, id: this.groupId })
                    .subscribe((res) => {
                        this.memberList = res;
                    });
            });
    }

    memberList: MemberItem[] = []

    editMember : MemberItem = {
        name: '',
        role: '',
        id: 0
    }

    startEdit(member : MemberItem) {
        this.editMember = member
    }
}
