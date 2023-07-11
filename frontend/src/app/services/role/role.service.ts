import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MemberRoleDTO } from 'src/DTO/MemberDTO/MemberDTO';
import { RoleDTO } from 'src/DTO/RoleDTO/RoleDTO';
import { GroupQuery } from 'src/models/Group';
import { BackendProviderService } from '../backendProvider/backend-provider.service';

@Injectable({
    providedIn: 'root',
})
export class RoleService {
    constructor(private http : HttpClient, private backendProvider : BackendProviderService) {}

    url : string = this.backendProvider.provide()

    createRole(roleData : RoleDTO) {
        return this.http.post(this.url + "/role", roleData);
    }

    getGroupRoles(groupData : GroupQuery) {
        return this.http.post<RoleDTO[]>(this.url + "/role/group-roles", groupData)
    }

    editRole(roleData : RoleDTO) {
        return this.http.put(this.url + "/role/", roleData)
    }

    removeRole(roleData : RoleDTO) { 
        return this.http.post(this.url + "/role/remove", roleData)
    }

    promoteMember(memberRoleData: MemberRoleDTO) {
        return this.http.post(
            this.url + '/role/promote-member',
            memberRoleData
        );
    }

    listPermissions(groupQuery: GroupQuery) {
        return this.http.post<number[]>(
            this.url + '/role/permission-list',
            groupQuery
        )
    }
}
