import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublishStoryComponent } from './publish-story.component';

describe('PublishStoryComponent', () => {
  let component: PublishStoryComponent;
  let fixture: ComponentFixture<PublishStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PublishStoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublishStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
