import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PostDTO } from 'src/DTO/PostDTO/PostDTO';
import { PostService } from '../services/post/post.service';
import { GroupService } from '../services/group/group.service';

@Component({
    selector: 'app-post-page',
    templateUrl: './post-page.component.html',
    styleUrls: ['./post-page.component.css'],
})
export class PostPageComponent implements OnInit, OnDestroy {
    constructor(
        private route: ActivatedRoute,
        private postService: PostService,
        private groupService: GroupService,
        private router: Router
    ) {}

    postData: PostDTO = {
        id: 0,
        jwt: '',
        title: '',
        content: '',
        groupId: 0,
        indexedImg: 0,
        authorName: '',
        groupName: '',
        likeCount: 0,
        postDate: new Date(),
        authorPhoto: 0
    };

    subscription: any;

    groupId: number = 0;
    
    getImgUrl = () : string => {
        return "http://localhost:5038/img/" + this.postData.indexedImg
    }

    ngOnInit(): void {
        let jwt = sessionStorage.getItem('jwtSession') ?? '';
        let groupName = this.router.url.split('/')[2];

        this.groupService
            .getGroup({ jwt: jwt, name: groupName })
            .subscribe((res) => {
                this.groupId = res.id;

                this.subscription = this.route.params.subscribe((params) => {
                    let postId: number = params['id'];
                        
                    this.postService
                        .getPost(jwt, postId, this.groupId)
                        .subscribe((res) => {
                            console.log(res)
                            this.postData = res;
                            console.log(this.postData)
                        });
                });
            });
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
