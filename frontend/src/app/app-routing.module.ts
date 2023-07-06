import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeedComponent } from './feed/feed.component';
import { HomeComponent } from './home/home.component';
import { PostPageComponent } from './post-page/post-page.component';
import { GroupPageComponent } from './group-page/group-page.component';
import { NotFoundComponent } from './not-found/not-found.component';

import { SignPageComponent } from './sign-page/sign-page.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { SignFormComponent } from './sign-form/sign-form.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { GroupCreatorComponent } from './group-creator/group-creator.component';
import { UserConfigComponent } from './user-config/user-config.component';
import { GroupSearchComponent } from './group-search/group-search.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { RoleManagerComponent } from './role-manager/role-manager.component';
import { MemberListComponent } from './member-list/member-list.component';

const routes: Routes = [
    {
        path: '',
        component: LandingPageComponent,
    },
    {
        path: 'home',
        component: HomeComponent,
    },
    {
        path: 'group-creator',
        component: GroupCreatorComponent,
    },
    {
        path: 'group-search',
        component: GroupSearchComponent,
    },
    {
        path: 'group/:name',
        component: GroupPageComponent,
        children: [
            { path: 'feed', component: FeedComponent },
            { path: 'post/:id', component: PostPageComponent },
            { path: 'post-creator', component: CreatePostComponent },
            { path: 'role-manager', component: RoleManagerComponent },
            { path: 'member-list', component: MemberListComponent },
        ],
    },
    {
        path: 'sign',
        title: 'Join us',
        component: SignPageComponent,
        children: [
            { path: 'in', component: SignFormComponent },
            { path: 'up', component: LoginFormComponent },
        ],
    },
    {
        path: 'configuration',
        title: 'Account Config',
        component: UserConfigComponent,
    },
    {
        path: '**',
        title: 'Not Found',
        component: NotFoundComponent,
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
