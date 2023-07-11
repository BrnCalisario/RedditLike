import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from 'src/models/User';
import { Injectable } from '@angular/core';
import { LoginDTO } from 'src/DTO/LoginDTO';
import { RegisterDTO } from 'src/DTO/RegisterDTO';
import { UserToken } from 'src/DTO/UserToken';
import { Jwt } from 'src/DTO/Jwt';
import { LoginResponse } from 'src/DTO/LoginResponse';
import { BackendProviderService } from '../backendProvider/backend-provider.service';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    constructor(private http: HttpClient, private backendProvider : BackendProviderService) {}

    url : string = this.backendProvider.provide()

    getUser(jwtSession: Jwt) {
        return this.http.post<User>(
            this.url + '/user/single',
            jwtSession
        );
    }

    login(loginData: LoginDTO) {
        return this.http.post<LoginResponse>(
            this.url + '/user/login',
            loginData
        );
    }

    register(registerData: RegisterDTO) {
        return this.http.post<number>(
            this.url + '/user/register',
            registerData
        );
    }

    validateJwt(jwt: Jwt) {
        return this.http.post<UserToken>(
            this.url + '/user/validate',
            jwt
        );
    }

    validateUser() {
        let session = sessionStorage.getItem('jwtSession') ?? '';

        return this.http.post<UserToken>(
            this.url + '/user/validate',
            { Value: session }
        );
    }
}
