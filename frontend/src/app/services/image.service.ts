import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Jwt } from 'src/DTO/Jwt';

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

    updateUserAvatar(form: FormData) {
        let jwt = sessionStorage.getItem('jwtSession') ?? '' 
        
        form.append('jwt', jwt)
        return this.http.post(
            'http://localhost:5038/img/add-avatar/',
            form,
            { observe: 'response' }
        );
    }
}
