import { Component, DoCheck, OnInit } from '@angular/core';
import { UserService } from './user.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
    
    authenticated : boolean = false;

    constructor(private userService: UserService) { }

    ngOnInit(): void {

        let jwtSession : string = sessionStorage.getItem("jwtSession") ?? ""
        
        this.userService.validateJwt({ jwt: jwtSession })        
            .subscribe(res => {
                console.log(res)
            })
    

        // try {
        //     this.userService.validateJwt({ jwt: jwtSession })
        //     .subscribe(res => {

        //         console.log(res)
        //     })
        // } catch (err) {
        //     console.log("oi")
        // }

    }


    title = 'Reddit';
}
