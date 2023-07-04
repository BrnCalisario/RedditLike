import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlternativeUploaderComponent } from './alternative-uploader.component';

describe('AlternativeUploaderComponent', () => {
  let component: AlternativeUploaderComponent;
  let fixture: ComponentFixture<AlternativeUploaderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AlternativeUploaderComponent]
    });
    fixture = TestBed.createComponent(AlternativeUploaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
