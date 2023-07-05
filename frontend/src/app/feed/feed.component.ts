import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from 'src/models/Post';
import { PostService } from '../services/post/post.service';
import { PostDTO } from 'src/DTO/PostDTO/PostDTO';

@Component({
    selector: 'app-feed',
    templateUrl: './feed.component.html',
    styleUrls: ['./feed.component.css'],
})
export class FeedComponent implements OnInit {
    constructor(private router: Router, private postService: PostService) {}

    isGroup : boolean = false;

    ngOnInit(): void {
        let url = this.router.url;

        let splited = url.split('/');
        let jwt = sessionStorage.getItem('jwtSession') ?? '';

        if (splited[1] === 'group') {
            this.isGroup = true;
            
            let groupName = splited[2];

            this.postService.getGroupFeedByName(jwt, groupName).subscribe({
                next: (res: PostDTO[]) => {
                    this.postList = res;
                    console.log(res)
                },
                error: (error: any) => {
                    console.log(error);
                },
            });
        }

        if (splited[1] == 'home') {
            this.postService.getMainFeed({ Value: jwt }).subscribe({
                next: (res: PostDTO[]) => {
                    this.postList = res;
                },
                error: (error: any) => {
                    console.log(error);
                },
            });
        }

    }

    postList: PostDTO[] = [];
}
