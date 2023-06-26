import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignFormComponent } from './sign-form.component';

describe('SignFormComponent', () => {
    let component: SignFormComponent;
    let fixture: ComponentFixture<SignFormComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [SignFormComponent],
        });
        fixture = TestBed.createComponent(SignFormComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
