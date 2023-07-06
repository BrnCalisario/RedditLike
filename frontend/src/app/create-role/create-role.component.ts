import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RoleService } from '../services/role/role.service';

@Component({
    selector: 'app-create-role',
    templateUrl: './create-role.component.html',
    styleUrls: ['./create-role.component.css'],
})
export class CreateRoleComponent {
    @Input() groupId: number = 0;

    @Output() public OnCreate = new EventEmitter<boolean>();

    constructor(private roleService: RoleService) {}

    roleName: string = '';

    deletePost: boolean = false;
    editPost: boolean = false;
    promoteMember: boolean = false;
    manageRoles: boolean = false;
    banMember: boolean = false;

    permissionList: number[] = [];

    createRole = () => {
        let jwt = sessionStorage.getItem('jwtSession') ?? '';

        this.roleService.createRole({
            jwt,
            id: 0,
            groupId: this.groupId,
            name: this.roleName,
            permissionsSet: this.permissionList,
        }).subscribe(res => {
            console.log("Deu certo: " + res)
            this.OnCreate.emit(true)
        });
    };

    onInputChange(event: any, btnValue: boolean) {
        let value: number = event.target.value;

        if (btnValue) this.permissionList.push(value);
        else
            this.permissionList = this.permissionList.filter(
                (d: number) => d !== value
            );
    }
}
