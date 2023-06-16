import { Component, Input, OnChanges } from '@angular/core';
import { Post } from 'src/models/Post';

@Component({
    selector: 'app-post',
    templateUrl: './post.component.html',
    styleUrls: ['./post.component.css'],
})
export class PostComponent {
    @Input() post: Post | undefined;

	
}
