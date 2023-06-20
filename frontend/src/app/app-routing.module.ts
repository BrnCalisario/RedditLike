import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeedComponent } from './feed/feed.component';
import { HomeComponent } from './home/home.component';
import { PostPageComponent } from './post-page/post-page.component';
import { GroupPageComponent } from './group-page/group-page.component';
import { NotFoundComponent } from './not-found/not-found.component';

const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent,
        children: [
            { path: 'feed', component: FeedComponent },
        ],
    },
    {
        path: 'group/:name', component: GroupPageComponent,
        children: [
            { path: 'feed', component: FeedComponent },
            { path: 'post', component: PostPageComponent },
        ]
    },
    {
        path: "**", title: "Not Found", component: NotFoundComponent 
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule { }
