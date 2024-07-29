import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadStoryComponent } from './read-story.component';

describe('ReadStoryComponent', () => {
  let component: ReadStoryComponent;
  let fixture: ComponentFixture<ReadStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReadStoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
