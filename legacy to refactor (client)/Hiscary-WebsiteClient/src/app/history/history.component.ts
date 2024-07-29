import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TestsharedService } from '../testshared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {

  constructor(private service: TestsharedService, private router: Router) { }

  StoryList: any = []
  CurrentUser: any = { id: 0 };
  IsLoading: boolean = true;

  ngOnInit(): void {
    if (this.service.isAuthenticated() == false)
    {
      this.router.navigateByUrl('login');
    }

    this.setUser();
    this.refreshStoryList();
  }

  refreshStoryList() {
    this.service.getHistory().subscribe(data => {
      this.StoryList = data.reverse();
      setTimeout(() => {
        this.IsLoading = false;
      }, 1000);
    })
  }

  setUser(): void {
    this.service.getUserInfo()
    .subscribe(res => {
      this.CurrentUser = res;
    },
    error => {
    });
  }
}
