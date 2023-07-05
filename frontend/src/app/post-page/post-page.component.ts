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

    post: PostDTO = {
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
        authorPhoto: 0,
        voteValue: 0,
    };

    subscription: any;

    groupId: number = 0;

    getIndexedImg = (): string => {
        return 'http://localhost:5038/img/' + this.post.indexedImg;
    };

    getUserAvatar = (): string => {
        let photoId = this.post.authorPhoto;
        if (photoId) return 'http://localhost:5038/img/' + photoId;

        return '../assets/image/avatar-placeholder.png';
    };

    upVoted: boolean = false;
    downVoted: boolean = false;

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
                            this.post = res;
                            console.log(this.post);

                            this.upVoted = this.post.voteValue == 1;
                            this.downVoted = this.post.voteValue == 2;
                        });
                });
            });
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    like = (): void => {
        console.log(this.post);

        let jwt = sessionStorage.getItem('jwtSession') ?? '';
        let postId = this.post.id;

        if (this.post.voteValue == 1) {
            console.log('Agui');
            this.postService
                .unvotePost({ jwt, postId, value: false })
                .subscribe((res) => {
                    window.location.reload();
                });
        } else {
            this.postService
                .votePost({ jwt, postId, value: true })
                .subscribe((res) => {
                    window.location.reload();
                });
        }

        console.log('Finalizou');
    };

    dislike = (): void => {
        let jwt = sessionStorage.getItem('jwtSession') ?? '';
        let postId = this.post.id;

        if (this.post.voteValue == 2) {
            this.postService
                .unvotePost({ jwt, postId, value: false })
                .subscribe((res) => {
                    window.location.reload();
                });
        } else {
            this.postService
                .votePost({ jwt, postId, value: false })
                .subscribe((res) => {
                    window.location.reload();
                });
        }

        console.log('Finalizou');
    };
}
