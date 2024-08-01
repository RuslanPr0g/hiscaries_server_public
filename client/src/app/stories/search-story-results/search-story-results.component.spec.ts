import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchStoryResultsComponent } from './search-story-results.component';

describe('SearchStoryResultsComponent', () => {
  let component: SearchStoryResultsComponent;
  let fixture: ComponentFixture<SearchStoryResultsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchStoryResultsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchStoryResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
