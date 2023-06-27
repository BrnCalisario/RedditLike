import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from 'src/models/User';
import { Injectable } from '@angular/core';
import { LoginDTO } from 'src/DTO/LoginDTO';
import { RegisterDTO } from 'src/DTO/RegisterDTO';
import { UserToken } from 'src/DTO/UserToken';
import { Jwt } from 'src/DTO/Jwt';

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

    validateJwt(jwt: Jwt) {
        return this.http.post<UserToken>('http://localhost:5038/validate', jwt, { 
            observe: 'response',
        })
    }
}
