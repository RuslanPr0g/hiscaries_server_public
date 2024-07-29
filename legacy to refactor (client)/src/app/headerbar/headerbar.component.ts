import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TestsharedService } from '../testshared.service';

@Component({
  selector: 'app-headerbar',
  templateUrl: './headerbar.component.html',
  styleUrls: ['./headerbar.component.css']
})
export class HeaderbarComponent implements OnInit {
  searchBarValue: string | null = '';
  isLoggedIn: boolean = false;
  CurrentUser: any;
  isPublisher: any;
  isAdmin: any;

  constructor(private router: Router, private service: TestsharedService, private route: ActivatedRoute) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }

  ngOnInit(): void {
    this.isLoggedIn = this.service.isAuthenticated();
    this.searchBarValue = this.route.snapshot.queryParamMap.get('query');

    if (this.service.isAuthenticated() == false)
    {
      this.router.navigateByUrl('login');
    }

    this.setUser();
  }

  redirectToSearchPage(event: any): void {
    let searchRequest = event.target.value.trim();

    if (searchRequest != '')
      this.router.navigateByUrl('story/search/all?s=' + searchRequest);
  }

  becomePub(): void {
    this.service.becomePublisher()
      .subscribe(res => {
      },
      error => {
      });
      
    this.router.navigateByUrl('publish');
  }

  setUser(): void {
    this.service.getUserInfo()
    .subscribe(res => {
      this.CurrentUser = res;
      this.isPublisher = this.CurrentUser.role === "publisher";
      this.isAdmin = this.CurrentUser.role === "admin";
    },
    error => {
    });
  }
}
