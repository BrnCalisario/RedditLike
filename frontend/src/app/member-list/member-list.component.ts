import { Component, OnInit } from '@angular/core';
import { MemberItem } from 'src/DTO/MemberDTO/MemberDTO';

@Component({
    selector: 'app-member-list',
    templateUrl: './member-list.component.html',
    styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
    ngOnInit(): void {}

    memberList : MemberItem[] = [{ name: "Jorge", role: "Moderador" }]
}
