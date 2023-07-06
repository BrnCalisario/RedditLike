import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GroupService } from '../services/group/group.service';
import { RoleDTO } from 'src/DTO/RoleDTO/RoleDTO';
import { RoleService } from '../services/role/role.service';

@Component({
    selector: 'app-role-manager',
    templateUrl: './role-manager.component.html',
    styleUrls: ['./role-manager.component.css'],
})
export class RoleManagerComponent implements OnInit{

    constructor(
        private router : Router, 
        private groupService : GroupService,
        private roleService : RoleService
        ) {}
    
    groupId : number = 0;
    subscription : any;

    roleList : RoleDTO[] = []

    editRole : RoleDTO = {
        jwt: '',
        id: 0,
        groupId: 0,
        name: '',
        permissionsSet: []
    }
    
    ngOnInit(): void {
        let name = this.router.url.split("/")[2]
        let jwt = sessionStorage.getItem("jwtSession") ?? ""

        this.groupService.getGroup({ jwt, name })
            .subscribe(res => {
                this.groupId = res.id
            })

        this.refreshRoles()
    }

    refreshRoles = () => {
        let name = this.router.url.split("/")[2]
        let jwt = sessionStorage.getItem("jwtSession") ?? ""
        
        this.roleService.getGroupRoles({ jwt, name })
        .subscribe(res => {
            console.log(res)
            this.roleList = res
        }) 
    }

    hasPermission(role : RoleDTO, number : number) {
        return role.permissionsSet.includes(number)
    }

    startEdit(role : RoleDTO)  {
        this.editRole = role
    }
}
