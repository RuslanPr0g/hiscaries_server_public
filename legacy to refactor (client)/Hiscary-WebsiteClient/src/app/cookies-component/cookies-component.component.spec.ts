import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CookiesComponentComponent } from './cookies-component.component';

describe('CookiesComponentComponent', () => {
  let component: CookiesComponentComponent;
  let fixture: ComponentFixture<CookiesComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CookiesComponentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CookiesComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
