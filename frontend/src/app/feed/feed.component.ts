import { Component, Input, OnInit } from '@angular/core';
import { Post } from 'src/models/Post';

@Component({
    selector: 'app-feed',
    templateUrl: './feed.component.html',
    styleUrls: ['./feed.component.css'],
})
export class FeedComponent {
    @Input() postList?: Post[] = [
        {
            id: 1,
            title: 'Titulo',
            content: 'Conteúdo',
            postDate: new Date(),
            author: 'BrnCalisario',
            group: 'Grupo dos gatos',
            likeCount: 2,
        },
    ];
}
