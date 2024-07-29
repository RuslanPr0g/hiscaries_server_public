import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowStoryComponent } from './show-story.component';

describe('ShowStoryComponent', () => {
  let component: ShowStoryComponent;
  let fixture: ComponentFixture<ShowStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowStoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
