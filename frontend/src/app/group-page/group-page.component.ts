import {
    Component,
    Input,
    OnInit,
    OnDestroy,
    AfterContentInit,
} from '@angular/core';
import { ActivatedRoute, NavigationStart, Router } from '@angular/router';
import { Group } from 'src/models/Group';
import { GroupService } from '../services/group/group.service';
import { HttpErrorResponse } from '@angular/common/http';
import { PostService } from '../services/post/post.service';
import { PostDTO } from 'src/DTO/PostDTO/PostDTO';

@Component({
    selector: 'app-group-page',
    templateUrl: './group-page.component.html',
    styleUrls: ['./group-page.component.css'],
})
export class GroupPageComponent implements AfterContentInit {
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private groupService: GroupService,
        private postService: PostService
    ) {}

    posts: PostDTO[] = [];

    group: Group = {
        id: 0,
        name: '',
        description: '',
        ownerId: 0,
        isMember: false,
        imageId: null,
        userQuantity: 0,
        jwt: '',
    };

    imgUrl = (): string => {
        if (this.group.imageId == null) return 'https://placehold.co/400';
        return 'http://localhost:5038/img/' + this.group.imageId;
    };

    subscription: any;

    ngOnInit() {
        this.subscription = this.route.params.subscribe((params) => {
            let groupName = params['name'];
            let jwt = sessionStorage.getItem('jwtSession') ?? '';

            this.groupService
                .getGroup({ jwt: jwt, name: groupName })
                .subscribe({
                    next: (group: Group) => {
                        this.group = group;
                        // this.postService
                        //     .getGroupFeedById(jwt, this.group.id)
                        //     .subscribe({
                        //         next: (res: PostDTO[]) => {
                        //             this.posts = res;
                        //             console.log(this.posts)
                        //         },
                        //         error: (error: HttpErrorResponse) => {
                        //             console.log(error);
                        //         },
                        //     });
                    },
                    error: (error: HttpErrorResponse) => {
                        this.router.navigate(['/404']);
                    },
                });
        });
    }

    ngAfterContentInit(): void {}

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    // @Input() group: Group = {
    //     name: 'Gatinhos',
    //     description: 'Grupo sobre gatinhos',
    // };
}
