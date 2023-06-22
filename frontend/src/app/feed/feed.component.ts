import { Component, OnInit } from '@angular/core';
import { Post } from 'src/models/Post';

@Component({
    selector: 'app-feed',
    templateUrl: './feed.component.html',
    styleUrls: ['./feed.component.css'],
})
export class FeedComponent {
    post: Post = {
        id: '1',
        title: 'Melhores marcas para areia de gato',
        content: 'Estou com dificuldade em achar a melhor areia para gato',
        author: 'BrnCalisario',
        group: 'Gatinhos',
        postDate: new Date(),
    };

    post2: Post = {
        id: '2',
        title: 'Estou em busca de rações para cachorros',
        content: 'Links uteis pls',
        author: 'BrnCalisario',
        group: 'Cachorros',
        postDate: new Date(),
    };


    feedPosts: Post[] = [this.post, this.post2];
}
