import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoStoryComponent } from './info-story.component';

describe('InfoStoryComponent', () => {
  let component: InfoStoryComponent;
  let fixture: ComponentFixture<InfoStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InfoStoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
