import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output, Input } from '@angular/core';

@Component({
    selector: 'app-uploader',
    templateUrl: './uploader.component.html',
    styleUrls: ['./uploader.component.css'],
})
export class UploaderComponent implements OnInit {
    @Output() public onUploadFinished = new EventEmitter<any>();
    @Input() public value: FormData = new FormData();

    constructor(private http: HttpClient) {}

    ngOnInit(): void {}

    imgUrl: string = '';

    uploadFile = (files: any) => {
        if (files.length == 0) {
            return;
        }

        let fileToUpload = <File>files[0];

        this.value = new FormData();
        this.value.append('file', fileToUpload, fileToUpload.name);
        this.imgUrl = URL.createObjectURL(fileToUpload);

        this.onUploadFinished.emit(this.value);
        // this.http.post('http://localhost:5038/img', formData)
        //     .subscribe(result => {
        //         this.onUploadFinished.emit(result);
        //     })
    };

    getImgSrc() {
        if (this.imgUrl !== '') return this.imgUrl;

        return '../assets/image/camera-icon.png';
    }
}
