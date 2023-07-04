import { Component } from '@angular/core';
import { PostDTO } from 'src/DTO/PostDTO/PostDTO';
import { PostService } from '../services/post/post.service';
import { Group } from 'src/models/Group';
import { GroupService } from '../services/group/group.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-create-post',
    templateUrl: './create-post.component.html',
    styleUrls: ['./create-post.component.css'],
})
export class CreatePostComponent {

    constructor(private router : Router,private postService: PostService, private groupService : GroupService)
    {
        let groupName = this.router.url.split("/")[2]
        let jwt = sessionStorage.getItem("jwtSession") ?? ""

        this.groupService.getGroup({ jwt: jwt, name: groupName})
            .subscribe(res => { this.group = res})
    }

    group?: Group;

    postForm : PostDTO = {
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

    imgForm : FormData = new FormData;

    onUpload(event: FormData) {
        this.imgForm = event;
    }

    createPost() {
        this.postForm.jwt = sessionStorage.getItem('jwtSession') ?? ""
        this.postForm.groupId = this.group?.id ?? 0

        this.postService.createPost(this.postForm)
            .subscribe({
                next: (res) => {
                    console.log(res)
                    let groupUrl = "group/" + this.group?.name + "/feed"
                    this.router.navigate([groupUrl])
                },
                error: (error: any) => {
                    console.log(error)
                }
            })
    }

}
