import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PostDTO } from 'src/DTO/PostDTO/PostDTO';
import { PostService } from '../services/post/post.service';
import { GroupService } from '../services/group/group.service';

@Component({
    selector: 'app-post',
    templateUrl: './post.component.html',
    styleUrls: ['./post.component.css'],
})
export class PostComponent {
    constructor() {}

    @Input() displayGroup: boolean = true;

    @Input() post: PostDTO = {
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
    };

    postLink = (): string => {
        return '';
    };
}
