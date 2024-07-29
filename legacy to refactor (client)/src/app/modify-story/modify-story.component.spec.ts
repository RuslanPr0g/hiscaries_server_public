import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifyStoryComponent } from './modify-story.component';

describe('ModifyStoryComponent', () => {
  let component: ModifyStoryComponent;
  let fixture: ComponentFixture<ModifyStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModifyStoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModifyStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
