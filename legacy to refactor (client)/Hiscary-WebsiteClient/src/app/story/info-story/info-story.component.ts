import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TestsharedService } from 'src/app/testshared.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-info-story',
  templateUrl: './info-story.component.html',
  styleUrls: ['./info-story.component.css']
})
export class InfoStoryComponent implements OnInit {

  snackBarDuration: number = 2000;
  ratingArr = [];

  constructor(private service: TestsharedService,
    private route: ActivatedRoute, private router: Router,
    private snackBar: MatSnackBar) { }

  Story: any = {};
  IsError: boolean = false;
  IsLoading: boolean = true;

  errorMessage = '';
  CommentList: any[] = [''];

  CurrentComment: string = '';

  User: any = {};

  ngOnInit(): void {
    if (this.service.isAuthenticated() == false) {
      this.router.navigateByUrl('login');
    }

    for (let index = 0; index < this.starCount; index++) {
      this.ratingArr.push(index as never);
    }

    let id = this.route.snapshot.params['id'];
    this.getStory(id);
    this.getUserInfo();
  }

  rating: number = 3;
  starCount: number = 5;

  getUserInfo(): any {
    this.service.getUserInfo().subscribe(data => {
      this.User = data;
      let isError = this.User == null || this.User == undefined ? true : false;

      if (isError) {
        setTimeout(() => {
          this.IsError = true;
        }, 1000);
      }

      setTimeout(() => {
        this.IsLoading = false;
      }, 1000);
    }, error => {
      this.IsError = true;
    })
  }

  writeComment() {
    if (this.CurrentComment.length > 850) {
      alert("Too long message!");
      return;
    }

    if (this.CurrentComment === '') {
      this.errorMessage = 'Please, write something!';
      setInterval(() => {
        this.errorMessage = '';
      }, 6000);
      return;
    }
    else {
      this.errorMessage = '';
    }

    this.service.writeComment({ storyId: this.Story.id, content: this.CurrentComment }).subscribe(data => {
      let isError = data == null;

      this.CurrentComment = '';
      this.refreshComments(this.Story.id);

      if (isError) {
        setTimeout(() => {
          this.IsError = true;
        }, 1000);
      }

      setTimeout(() => {
        this.IsLoading = false;
      }, 1000);
    }, error => {
      this.errorMessage = error.error;
      setInterval(() => {
        this.errorMessage = '';
      }, 6000);
    })
  }

  refreshComments(id: any) {
    this.service.getComments().subscribe(data => {
      this.CommentList = data.filter((s: any) => s.storyId == id);
    })
  }

  removeComment(id: any) {
    this.service.removeComment({ id }).subscribe(data => {
      this.refreshComments(this.Story.id);
    })
  }

  getStory(id: any): any {
    this.service.getStoryById({ id }).subscribe(data => {
      this.Story = data[0];
      let isError = this.Story == null || this.Story == undefined ? true : false;

      this.Story.genres = this.Story.genres.map(function (elem: any) {
        return elem.name;
      }).join(",");

      this.refreshComments(id);
      this.refreshScore();

      if (isError) {
        setTimeout(() => {
          this.IsError = true;
        }, 1000);
      }

      setTimeout(() => {
        this.IsLoading = false;
      }, 1000);
    }, error => {
      this.IsError = true;
    })
  }

  counter(i: number) {
    return new Array(i);
  }

  onClick(rating: number) {
    this.scoreStory(this.Story.id, rating);
    return false;
  }

  scoreStory(id: any, score: number) {
    this.service.scoryStore({ storyId: id, score }).subscribe(data => {
      this.refreshScore();
      this.snackBar.open('You rated ' + score + ' / ' + this.starCount, '', {
        duration: this.snackBarDuration
      });
      this.rating = score;
    }, error => {
      this.errorMessage = error.error;
      setInterval(() => {
        this.errorMessage = '';
      }, 6000);
    })
  }

  refreshScore() {
    this.service.storyScores().subscribe(data => {
      this.rating = data.filter((x: any) => x.userId == this.User.id &&
        x.storyId == this.Story.id)[0].score;
      this.refreshComments(this.Story.id);
    })
  }

  showIcon(index: number) {
    if (this.rating >= index + 1) {
      return 'star';
    } else {
      return 'star_border';
    }
  }

  showIcon100(index: number) {
    return 'star';
  }
}
