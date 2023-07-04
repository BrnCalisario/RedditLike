import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-alternative-uploader',
    templateUrl: './alternative-uploader.component.html',
    styleUrls: ['./alternative-uploader.component.css'],
})
export class AlternativeUploaderComponent {
    @Output() public onUploadFinished = new EventEmitter<any>();

    @Input() public value: FormData | undefined = new FormData();
    @Input() public title: string = '';
    @Input() public imgUrl: string = '';

    shouldRender: boolean = false;

    constructor() {}

    uploadFile = (files: any) => {
        if (files.length == 0) {
            return;
        }

        let fileToUpload = <File>files[0];

        this.value = new FormData();
        this.value.append('file', fileToUpload, fileToUpload.name);
        this.imgUrl = URL.createObjectURL(fileToUpload);

        this.onUploadFinished.emit(this.value);

        this.shouldRender = true;
    };

    clearInput() {
        this.shouldRender = false;
        this.value = new FormData();
        this.imgUrl = '';
    }

    getImgSrc() {
        if (this.imgUrl !== '') return this.imgUrl;

        return '../assets/image/placeholder.png';
    }
}
