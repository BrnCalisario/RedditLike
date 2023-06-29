import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from 'src/models/User';
import { Injectable } from '@angular/core';
import { LoginDTO } from 'src/DTO/LoginDTO';
import { RegisterDTO } from 'src/DTO/RegisterDTO';
import { UserToken } from 'src/DTO/UserToken';
import { Jwt } from 'src/DTO/Jwt';
import { LoginResponse } from 'src/DTO/LoginResponse';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    constructor(private http: HttpClient) {}

    getUser(jwtSession: Jwt) {
        return this.http.post<User>(
            'http://localhost:5038/user/single',
            jwtSession
        );
    }

    login(loginData: LoginDTO) {
        return this.http.post<LoginResponse>(
            'http://localhost:5038/user/login',
            loginData
        );
    }

    register(registerData: RegisterDTO) {
        return this.http.post<number>(
            'http://localhost:5038/user/register',
            registerData
        );
    }

    validateJwt(jwt: Jwt) {
        return this.http.post<UserToken>(
            'http://localhost:5038/user/validate',
            jwt
        );
    }

    validateUser() {
        let session = sessionStorage.getItem('jwtSession') ?? '';

        return this.http.post<UserToken>(
            'http://localhost:5038/user/validate',
            { Value: session }
        );
    }
}
