import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  styleUrls: ['./uploader.component.css']
})
export class UploaderComponent implements OnInit {

  @Output() public onUploadFinished = new EventEmitter<any>();

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    
  }

  uploadFile = (files: any) => {
    if(files.length == 0) {
      return;
    }

    // let fileToUpload = <File>files[0];
    // const formData = new FormData();

    // formData.append('file', fileToUpload, fileToUpload.name);

    // this.http.post('http/localhost:5038/img', formData)
    //   .subscribe(result => 
    //     {
    //       this.onUploadFinished.emit(result);
    //     })

  }
}
