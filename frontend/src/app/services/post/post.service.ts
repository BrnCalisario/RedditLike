import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Jwt } from 'src/DTO/Jwt';
import { CommentDTO, PostDTO, VoteDTO } from 'src/DTO/PostDTO/PostDTO';
import { Group, GroupQuery } from 'src/models/Group';

@Injectable({
    providedIn: 'root',
})
export class PostService {
    constructor(private http: HttpClient) {}

    getPost(jwt: string, postId : number, groupId: number) {
        return this.http.post<PostDTO>('http://localhost:5038/post/single', { jwt, id: postId, groupId  });
    }

    createPost(postData: PostDTO) {
        return this.http.post<number>('http://localhost:5038/post', postData);
    }

    updatePost(postData: PostDTO) {
        return this.http.put('http://localhost:5038/post', postData);
    }

    deletePost(postData: PostDTO) {
        return this.http.post('http://localhost:5038/post/remove', postData);
    }

    votePost(voteData: VoteDTO) {
        return this.http.post('http://localhost:5038/post/vote', voteData);
    }

    unvotePost(voteData: VoteDTO) {
        return this.http.post('http://localhost:5038/post', voteData);
    }

    commentPost(commentPost: CommentDTO) {
        return this.http.post('http://localhost:5038/comment', commentPost);
    }

    removeComment(commentPost: CommentDTO) {
        return this.http.post(
            'http://localhost:5038/delete-comment',
            commentPost
        );
    }

    getMainFeed(jwt: Jwt) {
        return this.http.post<PostDTO[]>(
            'http://localhost:5038/post/main-feed',
            jwt
        );
    }

    getGroupFeedById(jwt: string, groupId: number) {
        return this.http.post<PostDTO[]>(
            'http://localhost:5038/post/group-feed/id',
            { jwt: jwt, id: groupId }
        );
    }

    getGroupFeedByName(jwt: string, groupName: string) {
        return this.http.post<PostDTO[]>(
            'http://localhost:5038/post/group-feed/group-name',
            { jwt: jwt, name: groupName }
        );
    }
}
