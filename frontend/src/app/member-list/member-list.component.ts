import { Component, OnInit } from '@angular/core';
import { MemberItem, MemberRoleDTO } from 'src/DTO/MemberDTO/MemberDTO';
import { GroupService } from '../services/group/group.service';
import { Router } from '@angular/router';
import { RoleService } from '../services/role/role.service';

@Component({
    selector: 'app-member-list',
    templateUrl: './member-list.component.html',
    styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
    constructor(
        private groupService: GroupService, 
        private router: Router,
        private roleService : RoleService
        ) {}

    groupId: number = 0;
    jwt = sessionStorage.getItem('jwtSession') ?? '';

    userPermissions : number[] = []

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

                this.roleService.listPermissions({ jwt: this.jwt, id: this.groupId })
                    .subscribe((res) => {
                        this.userPermissions = res
                    })
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

    hasPermission(permission : number) {
        return this.userPermissions.includes(permission)
    }

    removeMember(member: MemberItem) {
        if(!confirm("Tem certeza que deseja remover este usuário ?"))
            return
        console.log(member.id)

        let result : MemberRoleDTO = {
            jwt: this.jwt,
            userId: member.id,
            roleId: 0,
            groupId: this.groupId
        }

        this.groupService.removeMember(result)
            .subscribe(res => {
                window.location.reload()
            })
    }
}
