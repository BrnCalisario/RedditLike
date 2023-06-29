import { Component, OnInit } from '@angular/core';
import { GroupService } from '../services/group.service';
import { Group } from 'src/models/Group';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
	selector: 'app-group-search',
	templateUrl: './group-search.component.html',
	styleUrls: ['./group-search.component.css']
})
export class GroupSearchComponent implements OnInit {

	constructor(private groupService: GroupService) { }

	groupList : Group[] = []

	ngOnInit(): void {
		
		let jwt = sessionStorage.getItem('jwtSession') ?? ''

		this.groupService.listGroups({ Value : jwt})
			.subscribe({
				next: (res: Group[]) => {
					console.log(res)
					this.groupList = res
				},
				error: (error: HttpErrorResponse) => {
					console.log(error)
				}
			})
	}



}
