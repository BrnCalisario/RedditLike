import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BackendProviderService {

  constructor() { }

  provide() : string {
    return "http://localhost:5038"
  }
}
