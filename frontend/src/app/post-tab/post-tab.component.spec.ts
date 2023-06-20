import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostTabComponent } from './post-tab.component';

describe('PostTabComponent', () => {
    let component: PostTabComponent;
    let fixture: ComponentFixture<PostTabComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [PostTabComponent],
        });
        fixture = TestBed.createComponent(PostTabComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
