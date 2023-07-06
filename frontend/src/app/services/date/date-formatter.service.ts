import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateFormatterService {

  constructor() { }

  formatDate(date : Date) : string {
    
    let result = ""
    let data = new Date(date);
    
    if(data.toDateString() == new Date().toDateString()) {
      result = "Hoje, às " + data.toTimeString().substring(0, 5)
    } else {
      result = data.toLocaleDateString()
    }

    return result
  }
}
