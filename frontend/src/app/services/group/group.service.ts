import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Jwt } from 'src/DTO/Jwt';
import { MemberDTO, MemberItem, MemberRoleDTO } from 'src/DTO/MemberDTO/MemberDTO';
import { Group, GroupQuery } from 'src/models/Group';
import { BackendProviderService } from '../backendProvider/backend-provider.service';

@Injectable({
    providedIn: 'root',
})
export class GroupService {
    constructor(private http: HttpClient, private backendProvider : BackendProviderService) {}

    url : string = this.backendProvider.provide()

    getGroup(groupQuery: GroupQuery) {
        return this.http.post<Group>(
            this.url + "/group/by-name",
            groupQuery
        );
    }

    postGroup(group: Group) {
        return this.http.post<number>(this.url + '/group/', group);
    }

    updateGroup(group: Group) {
        return this.http.put(this.url + '/group/', group);
    }

    deleteGroup(group: Group) {
        return this.http.post(this.url + '/group/remove', group);
    }

    enterGroup(memberData: MemberDTO) {
        return this.http.post(this.url + '/group/enter', memberData);
    }

    quitGroup(memberData: MemberDTO) {
        return this.http.post(this.url + '/group/exit', memberData);
    }

    removeMember(memberData: MemberRoleDTO) {
        return this.http.post(
            this.url + '/group/remove-member',
            memberData
        );
    }

    listGroups(jwt: Jwt) {
        return this.http.post<Group[]>(this.url + '/group/list', jwt);
    }

    listMembers(groupQuery : GroupQuery) {
        return this.http.post<MemberItem[]>(this.url + '/group/group-members', groupQuery)
    }
}
