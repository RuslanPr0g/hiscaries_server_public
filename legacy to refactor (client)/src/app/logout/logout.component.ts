import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TestsharedService } from '../testshared.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(private router: Router, private service: TestsharedService) { 
    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }

  ngOnInit(): void {
    this.service.logOut();
    this.router.navigateByUrl('');
  }
}
