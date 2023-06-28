import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Group } from 'src/models/Group';

@Component({
    selector: 'app-group-page',
    templateUrl: './group-page.component.html',
    styleUrls: ['./group-page.component.css'],
})
export class GroupPageComponent {
    constructor(private route: ActivatedRoute, private router: Router) {}

    // subscription: any;

    // ngOnInit() {
    //     this.subscription = this.route.params.subscribe((params) => {
    //         this.group = {
    //             name: params['name'],
    //             description: 'Descrição do grupo',
    //         };
    //     });
    // }

    // ngOnDestroy() {
    //     this.subscription.unsubscribe();
    // }

    // @Input() group: Group = {
    //     name: 'Gatinhos',
    //     description: 'Grupo sobre gatinhos',
    // };
}
