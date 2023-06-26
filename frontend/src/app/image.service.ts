import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class ImageService {
    constructor(private http: HttpClient) {}

    getImage(id: number) {
        return this.http.get('http://localhost:5038/img/' + id);
    }

    uploadImage(form: FormData) {
        return this.http.post('http://localhost:5038/img', form);
    }
}
