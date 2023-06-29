import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Jwt } from 'src/DTO/Jwt';
import { Group } from 'src/models/Group';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http: HttpClient) {

  }


  postGroup(group: Group) {
    return this.http.post('http://localhost:5038/group/', group);
  }

  listGroups(jwt: Jwt) {
    return this.http.post<Group[]>('http://localhost:5038/group/list', jwt);
  }
}
