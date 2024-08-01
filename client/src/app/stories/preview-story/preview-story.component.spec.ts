import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewStoryComponent } from './preview-story.component';

describe('PreviewStoryComponent', () => {
  let component: PreviewStoryComponent;
  let fixture: ComponentFixture<PreviewStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PreviewStoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PreviewStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
