import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SidenavComponent } from './sidenav/sidenav.component';

import { MatButtonModule } from '@angular/material/button';
import { HomeComponent } from './home/home.component';
import { FeedComponent } from './feed/feed.component';
import { PostComponent } from './post/post.component';
import { GroupListComponent } from './group-list/group-list.component';

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        HomeComponent,
        FeedComponent,
        PostComponent,
        GroupListComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatButtonModule,
        SidenavComponent,
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}
