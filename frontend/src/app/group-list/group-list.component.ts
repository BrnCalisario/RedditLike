import { Component, Input } from '@angular/core';
import { Group } from 'src/models/Group';

@Component({
    selector: 'app-group-list',
    templateUrl: './group-list.component.html',
    styleUrls: ['./group-list.component.css'],
})
export class GroupListComponent {
    @Input() groupList? : Group[] ;

    // groups: string [] = []
}
