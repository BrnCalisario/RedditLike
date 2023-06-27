import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

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
import { PostPageComponent } from './post-page/post-page.component';
import { GroupPageComponent } from './group-page/group-page.component';
import { PostTabComponent } from './post-tab/post-tab.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { SignPageComponent } from './sign-page/sign-page.component';
import { UploaderComponent } from './uploader/uploader.component';
import { FormsModule } from '@angular/forms';
import { LoginFormComponent } from './login-form/login-form.component';
import { SignFormComponent } from './sign-form/sign-form.component';
import { VoteButtonComponent } from './vote-button/vote-button.component';
import { LandingPageComponent } from './landing-page/landing-page.component';

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        HomeComponent,
        FeedComponent,
        PostComponent,
        GroupListComponent,
        PostPageComponent,
        GroupPageComponent,
        PostTabComponent,
        NotFoundComponent,
        SignPageComponent,
        UploaderComponent,
        LoginFormComponent,
        SignFormComponent,
        VoteButtonComponent,
        LandingPageComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatButtonModule,
        SidenavComponent,
        FormsModule,
        HttpClientModule,
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}
