import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Jwt } from 'src/DTO/Jwt';
import { CommentDTO, PostDTO, VoteDTO } from 'src/DTO/PostDTO/PostDTO';
import { Group, GroupQuery } from 'src/models/Group';
import { BackendProviderService } from '../backendProvider/backend-provider.service';

@Injectable({
    providedIn: 'root',
})
export class PostService {
    constructor(private http: HttpClient, private backendProvider : BackendProviderService) {}

    url : string = this.backendProvider.provide()
    
    getPost(jwt: string, postId: number, groupId: number) {
        return this.http.post<PostDTO>(this.url + '/post/single', {
            jwt,
            id: postId,
            groupId,
        });
    }

    createPost(postData: PostDTO) {
        return this.http.post<number>(this.url + '/post', postData);
    }

    updatePost(postData: PostDTO) {
        return this.http.put(this.url + '/post', postData);
    }

    deletePost(postData: PostDTO) {
        return this.http.post(this.url + '/post/remove', postData);
    }

    votePost(voteData: VoteDTO) {
        return this.http.post(this.url + '/post/vote', voteData);
    }

    unvotePost(voteData: VoteDTO) {
        return this.http.post(this.url + '/post/undo', voteData);
    }

    commentPost(commentPost: CommentDTO) {
        return this.http.post(this.url + '/comment', commentPost);
    }

    removeComment(commentPost: CommentDTO) {
        return this.http.post(
            this.url + '/delete-comment',
            commentPost
        );
    }

    getMainFeed(jwt: Jwt) {
        return this.http.post<PostDTO[]>(
            this.url + '/post/main-feed',
            jwt
        );
    }

    getGroupFeedById(jwt: string, groupId: number) {
        return this.http.post<PostDTO[]>(
            this.url + '/post/group-feed/id',
            { jwt: jwt, id: groupId }
        );
    }

    getGroupFeedByName(jwt: string, groupName: string) {
        return this.http.post<PostDTO[]>(
            this.url + '/post/group-feed/group-name',
            { jwt: jwt, name: groupName }
        );
    }
}
