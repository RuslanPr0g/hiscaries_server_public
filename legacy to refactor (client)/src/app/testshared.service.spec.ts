import { TestBed } from '@angular/core/testing';

import { TestsharedService } from './testshared.service';

describe('TestsharedService', () => {
  let service: TestsharedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TestsharedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
