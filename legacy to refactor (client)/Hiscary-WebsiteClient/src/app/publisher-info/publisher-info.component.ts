import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TestsharedService } from 'src/app/testshared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-publisher-info',
  templateUrl: './publisher-info.component.html',
  styleUrls: ['./publisher-info.component.css']
})
export class PublisherInfoComponent implements OnInit {

  constructor(private service: TestsharedService, 
    private route: ActivatedRoute, private router: Router) { }

    User: any = {};
    CurrentId: any = 0;
    Username: string = '';
    IsError: boolean = false;
    IsLoading: boolean = true;
    errorMessage: string = '';

  StoryList: any = []
  ReviewList: any = []
  CurrentReview: string = '';

  ngOnInit(): void {
    this.Username = this.route.snapshot.params['username'];
    this.CurrentId = this.route.snapshot.params['id'];
    this.getUserInfo();
    this.refreshStoryList();
  }

  getUserInfo(): any {
    this.service.getUserInfoUsername(this.Username).subscribe(data => {
        this.User = data;
        this.refreshReviews(this.User.id);

        setTimeout(() => {
          this.IsLoading = false;
        }, 1000);
    }, error => {
      this.IsError = true;
    })

    this.service.getUserInfo()
    .subscribe(res => {
      this.CurrentId = res.id;
    },
    error => {
    });
  }

  addReview() {
    if (this.CurrentReview.length > 850) {
      alert("Too long message!");
      return;
    }

    this.service.publishReview({ publisherId: this.User.id,
      reviewerId: this.CurrentId, message: this.CurrentReview}).subscribe(data => {
      let isError = data == null;

      this.CurrentReview = '';
      this.refreshReviews(this.User.id);

      if (isError) {
        setTimeout(() => {
          this.IsError = true;
        }, 1000);
      }

      setTimeout(() => {
        this.IsLoading = false;
      }, 1000);
    }, error => {
      alert(error.error)
    })
  }

  refreshReviews(id: any) {
    this.service.getReviews().subscribe(data => {
      this.ReviewList = data.filter((s: any) => s.publisherId == id);
    })
  }

  removeReview(itemId: any) {
    this.service.removeReview({ id: itemId }).subscribe(data => {
      this.refreshReviews(this.User.id);
    })
  }

  bookmark(storyId: any) {
    this.service.bookmarkStory({ storyId })
    .subscribe(res => {
      alert("Bookmark added!");
    },
    error => {
    });
  }

  refreshStoryList() {
    this.service.getStoryList().subscribe(data => {
      this.StoryList = data.filter(item => item.publisherId == this.User.id).reverse();

      setTimeout(() => {
        this.IsLoading = false;
      }, 1000);
    })
  }
}
