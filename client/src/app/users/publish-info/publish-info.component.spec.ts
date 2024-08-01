import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublishInfoComponent } from './publish-info.component';

describe('PublishInfoComponent', () => {
  let component: PublishInfoComponent;
  let fixture: ComponentFixture<PublishInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublishInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublishInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
