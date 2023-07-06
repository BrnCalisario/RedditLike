import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RoleDTO } from 'src/DTO/RoleDTO/RoleDTO';
import { GroupQuery } from 'src/models/Group';

@Injectable({
    providedIn: 'root',
})
export class RoleService {
    constructor(private http : HttpClient) {}

    createRole(roleData : RoleDTO) {
        return this.http.post("http://localhost:5038/role", roleData);
    }

    getGroupRoles(groupData : GroupQuery) {
        return this.http.post<RoleDTO[]>("http://localhost:5038/role/group-roles", groupData)
    }

    editRole(roleData : RoleDTO) {
        return this.http.put("http://localhost:5038/role/", roleData)
    }

    removeRole(roleData : RoleDTO) { 
        return this.http.post("http://localhost:5038/role/remove", roleData)
    }
}
