import { HttpClient } from "@angular/common/http";
import { User } from "src/models/User";
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService {

  constructor(private http: HttpClient) { }
  
}
