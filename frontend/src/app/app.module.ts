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
import { GroupCreatorComponent } from './group-creator/group-creator.component';
import { UserConfigComponent } from './user-config/user-config.component';
import { GroupSearchComponent } from './group-search/group-search.component';
import { UserCardComponent } from './user-card/user-card.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { AlternativeUploaderComponent } from './alternative-uploader/alternative-uploader.component';
import { RoleManagerComponent } from './role-manager/role-manager.component';
import { MemberListComponent } from './member-list/member-list.component';
import { CreateRoleComponent } from './create-role/create-role.component';
import { EditRoleComponent } from './edit-role/edit-role.component';
import { EditMemberComponent } from './edit-member/edit-member.component';

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
        GroupCreatorComponent,
        UserConfigComponent,
        GroupSearchComponent,
        UserCardComponent,
        CreatePostComponent,
        AlternativeUploaderComponent,
        RoleManagerComponent,
        MemberListComponent,
        CreateRoleComponent,
        EditRoleComponent,
        EditMemberComponent,
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
