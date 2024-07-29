import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditStoryComponent } from './add-edit-story.component';

describe('AddEditStoryComponent', () => {
  let component: AddEditStoryComponent;
  let fixture: ComponentFixture<AddEditStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddEditStoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
