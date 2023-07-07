import { Component, Input, OnInit } from '@angular/core';
import { MemberItem } from 'src/DTO/MemberDTO/MemberDTO';
import { RoleDTO } from 'src/DTO/RoleDTO/RoleDTO';
import { RoleService } from '../services/role/role.service';
import { GroupService } from '../services/group/group.service';
import { Router } from '@angular/router';
import { group } from '@angular/animations';

@Component({
    selector: 'app-edit-member',
    templateUrl: './edit-member.component.html',
    styleUrls: ['./edit-member.component.css'],
})
export class EditMemberComponent implements OnInit {
    constructor(
        private roleService: RoleService,
        private groupService: GroupService,
        private router: Router
    ) {}

    ngOnInit(): void {
        let groupName = this.router.url.split('/')[2];

        this.roleService
            .getGroupRoles({ jwt: this.jwt, name: groupName })
            .subscribe((res) => [
                (this.roleOptions = res.filter((x) => x.name !== 'Admin')),
            ]);
    }

    @Input() editMember: MemberItem = {
        name: '',
        role: '',
        id: 0,
    };

    @Input() groupId? : number;

    selectedValue: RoleDTO = {
        jwt: '',
        id: 0,
        groupId: 0,
        name: '',
        permissionsSet: [],
    };

    jwt: string = sessionStorage.getItem('jwtSession') ?? '';

    confirmEdit() {
        this.roleService.promoteMember({ 
            memberId: this.editMember.id,
            jwt: this.jwt,
            roleId : this.selectedValue.id,
            groupId: this.groupId ?? 0
        }).subscribe(res => {
            console.log("Mudou o cargo" + res)
            window.location.reload()
        });
    }

    roleOptions: RoleDTO[] = [];
}
