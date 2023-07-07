import {
    Component,
    Input,
    OnInit,
    OnDestroy,
    AfterContentInit,
} from '@angular/core';
import {
    ActivatedRoute,
    NavigationEnd,
    NavigationStart,
    Router,
} from '@angular/router';
import { Group, Permission } from 'src/models/Group';
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
    ) {
        router.events.subscribe((val) => {
            if (val instanceof NavigationEnd) {
                let subRoute = this.router.url.split('/')[3];

                this.onFeed = subRoute === 'feed';
                this.onCreator = subRoute === 'post-creator';
            }
        });
    }

    posts: PostDTO[] = [];

    onFeed: boolean = true;
    onCreator: boolean = true;

    group: Group = {
        id: 0,
        name: '',
        description: '',
        ownerId: 0,
        isMember: false,
        imageId: null,
        userQuantity: 0,
        jwt: '',
        userRole: '',
        userPermissions: []
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

    hasPermission(permission : number)
    {
        return this.group.userPermissions.includes(permission)
    }


    quitGroup()
    {
        if(!confirm("Tem certeza que deseja sair do grupo ?"))
            return

        let jwt = sessionStorage.getItem("jwtSession") ?? ""
        this.groupService.quitGroup({jwt, groupId: this.group.id})
            .subscribe(res => {
                this.router.navigate(["/home"])
            })
    }
}
