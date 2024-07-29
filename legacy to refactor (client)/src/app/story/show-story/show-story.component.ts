import { Component, OnInit } from '@angular/core';
import { TestsharedService } from 'src/app/testshared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-show-story',
  templateUrl: './show-story.component.html',
  styleUrls: ['./show-story.component.css']
})
export class ShowStoryComponent implements OnInit {

  constructor(private service: TestsharedService, private router: Router) { }

  StoryList: any = []
  CurrentUser: any = { id: 0 };

  ngOnInit(): void {
    if (this.service.isAuthenticated() == false)
    {
      this.router.navigateByUrl('login');
    }

    this.setUser();
    this.refreshStoryList();
  }

  refreshStoryList() {
    this.service.getStoryList().subscribe(data => {
      this.StoryList = data.reverse();
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

  bookmark(storyId: any) {
    this.service.bookmarkStory({ storyId })
    .subscribe(res => {
      alert("Bookmark added!");
    },
    error => {
    });
  }
}
