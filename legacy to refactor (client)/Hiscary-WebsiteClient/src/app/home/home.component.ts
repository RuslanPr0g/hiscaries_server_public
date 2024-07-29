import { Component, OnInit } from '@angular/core';
import { TestsharedService } from 'src/app/testshared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  constructor(private service: TestsharedService, private router: Router) {
    this.cookieMessageDismissed = JSON.parse(
      localStorage.getItem('cookieMessageDismissed') || 'false'
    );
  }

  cookieMessageDismissed: boolean;

  GenreList: any = [];

  ngOnInit(): void {
    this.setGenres();
  }

  openCookies() {
    window.open('/cookies', '_blank');
    this.dismissCookieMessage();
  }

  dismissCookieMessage() {
    this.cookieMessageDismissed = true;
    localStorage.setItem('cookieMessageDismissed', JSON.stringify(true));
  }

  setGenres(): void {
    this.service.getGenres().subscribe((data) => {
      this.GenreList = data.reverse();
    });
  }
}
