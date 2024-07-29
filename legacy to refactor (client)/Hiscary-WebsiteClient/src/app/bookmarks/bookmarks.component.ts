import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TestsharedService } from '../testshared.service';

@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.component.html',
  styleUrls: ['./bookmarks.component.css']
})
export class BookmarksComponent implements OnInit {

  constructor(private service: TestsharedService, private router: Router) {

   }

  ngOnInit(): void {
    if (this.service.isAuthenticated() === false) {
      this.router.navigateByUrl('login');
    }

    this.refreshStoryList();
  }

  StoryList: any = []

  refreshStoryList() {
    this.service.getBookmarks().subscribe(data => {
      this.StoryList = data.reverse();
    })
  }

  bookmark(storyId: any) {
    this.service.bookmarkStory({ storyId })
    .subscribe(res => {
      this.refreshStoryList();
    },
    error => {
    });
  }
}
