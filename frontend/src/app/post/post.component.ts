import { Component, Input, OnChanges } from '@angular/core';
import { PostDTO } from 'src/DTO/PostDTO/PostDTO';
import { Post } from 'src/models/Post';

@Component({
    selector: 'app-post',
    templateUrl: './post.component.html',
    styleUrls: ['./post.component.css'],
})
export class PostComponent {
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
        postDate: new Date()
    }
}
