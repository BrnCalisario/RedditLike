import { HttpClient } from '@angular/common/http';
import { User } from 'src/models/User';
import { Injectable } from '@angular/core';
import { LoginDTO } from 'src/DTO/LoginDTO';
import { RegisterDTO } from 'src/DTO/RegisterDTO';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    constructor(private http: HttpClient) { }

    login(loginData: LoginDTO) {
        return this.http.post('http://localhost:5038/login', loginData, {
            observe: 'response',
        });
    }

    register(registerData: RegisterDTO) {
        return this.http.post('http://localhost:5038/register/', registerData, {
            observe: 'response',
        });
    }

    validateJwt(jwt: string) {
        return this.http.post('http://localhost:5038/validate', jwt, { 
            observe: 'response' 
        })
    }
}
