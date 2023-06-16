import { Component, OnInit } from '@angular/core';
import { Post } from 'src/models/Post'

@Component({
    selector: 'app-feed',
    templateUrl: './feed.component.html',
    styleUrls: ['./feed.component.css'],
})
export class FeedComponent {
    post: Post = {
        id: '1',
        title: 'Post Title',
        content: 'Content',
        author: 'BrnCalisario',
        group: 'Gatinhos',
        postDate: new Date(),
    };
    feedPosts: Post[] = [this.post, this.post, this.post];
}
