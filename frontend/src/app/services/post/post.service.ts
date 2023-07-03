import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommentDTO, PostDTO, VoteDTO } from 'src/DTO/PostDTO/PostDTO';

@Injectable({
    providedIn: 'root',
})
export class PostService {
	constructor(private http: HttpClient) {}
	
    createPost(postData: PostDTO) {
		return this.http.post("http://localhost:5038/post", postData)
	}
	
	updatePost(postData: PostDTO) {
		return this.http.put("http://localhost:5038/post", postData)
	}

	deletePost(postData : PostDTO) {
		return this.http.post("http://localhost:5038/post/remove", postData)
	}

	votePost(voteData : VoteDTO) {
		return this.http.post("http://localhost:5038/post/vote", voteData)
	}

	unvotePost(voteData : VoteDTO) {
		return this.http.post("http://localhost:5038/post", voteData)
	}

	commentPost(commentPost: CommentDTO) {
		return this.http.post("http://localhost:5038/comment", commentPost)
	}

	removeComment(commentPost : CommentDTO) {
		return this.http.post("http://localhost:5038/delete-comment", commentPost)
	}

}


