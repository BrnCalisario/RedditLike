import { Component, DoCheck } from '@angular/core';
import { UserService } from './user.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
})
export class AppComponent implements DoCheck{
    

    constructor(private userService: UserService) { }

    ngDoCheck(): void {

        let jwtSession : string | null = sessionStorage.getItem("jwtSession")
        
        if(!jwtSession)
        {
            return
        }

        this.userService.validateJwt(jwtSession)
            .subscribe()
    }


    title = 'Reddit';
}
