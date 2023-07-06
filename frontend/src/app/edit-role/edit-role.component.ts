import { Component, Input, OnChanges } from '@angular/core';
import { RoleDTO } from 'src/DTO/RoleDTO/RoleDTO';
import { RoleService } from '../services/role/role.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-edit-role',
    templateUrl: './edit-role.component.html',
    styleUrls: ['./edit-role.component.css'],
})
export class EditRoleComponent implements OnChanges {
    
    constructor(private roleService : RoleService, private router : Router) {

    }
    
    @Input() editRole: RoleDTO = {
        jwt: '',
        id: 0,
        groupId: 0,
        name: '',
        permissionsSet: [],
    };

    deletePost: boolean = false;
    editPost: boolean = false;
    promoteMember: boolean = false;
    manageRoles: boolean = false;
    banMember: boolean = false;

    ngOnChanges() : void {
        console.log("VAI")
        this.deletePost = this.editRole.permissionsSet.includes(2)
        this.editPost = this.editRole.permissionsSet.includes(3)
        this.promoteMember = this.editRole.permissionsSet.includes(4)
        this.manageRoles = this.editRole.permissionsSet.includes(5)
        this.banMember = this.editRole.permissionsSet.includes(6)
    }

    onInputChange(event: any, btnValue: boolean) {
        
        let value: string = event.target.value;
        
        if (btnValue) this.editRole.permissionsSet.push(parseInt(value));
        else
        this.editRole.permissionsSet = this.editRole.permissionsSet.filter(
            (v: number) => v != parseInt(value)
            );


        console.log(this.editRole.permissionsSet)
    }

    editChanges() : void {
        let jwt = sessionStorage.getItem("jwtSession") ?? ""

        this.editRole.jwt = jwt

        this.roleService.editRole(this.editRole)
            .subscribe({
                next: (res : any) => {
                    window.location.reload();
                },
                error: (err : any) => {
                    console.log(err)
                }
            })
    }
}
