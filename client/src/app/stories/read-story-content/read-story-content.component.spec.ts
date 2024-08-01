import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadStoryContentComponent } from './read-story-content.component';

describe('ReadStoryContentComponent', () => {
  let component: ReadStoryContentComponent;
  let fixture: ComponentFixture<ReadStoryContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReadStoryContentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReadStoryContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
