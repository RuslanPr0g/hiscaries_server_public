import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublisherInfoComponent } from './publisher-info.component';

describe('PublisherInfoComponent', () => {
  let component: PublisherInfoComponent;
  let fixture: ComponentFixture<PublisherInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublisherInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublisherInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
