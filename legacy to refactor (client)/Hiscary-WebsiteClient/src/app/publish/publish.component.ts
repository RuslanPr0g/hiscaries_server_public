import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TestsharedService } from '../testshared.service';
import { Genre } from './Genre';

@Component({
  selector: 'app-publish',
  templateUrl: './publish.component.html',
  styleUrls: ['./publish.component.css']
})
export class PublishComponent implements OnInit {
  errorMessage: string = '';
  publishstory: FormGroup;

  selectedValue: number[] = [];

  genres: Genre[] = [];

  constructor(private formBuilder: FormBuilder, private service: TestsharedService, private router: Router) {
    this.publishstory = this.formBuilder.group({
      title: '',
      description: '',
      authorname: '',
      genreid: '',
      agelimit: '',
      datewritten: new Date()
    })

    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }

  GenreList: any = [];

  ngOnInit(): void {
    if (this.service.isAuthenticated() == false) {
      this.router.navigateByUrl('login');
    }

    this.setGenres();
  }

  setGenres(): void {
    this.service.getGenres().subscribe(data => {
      this.GenreList = data.reverse();

      this.GenreList.forEach((genre: any) => {
        this.genres.push({ id: genre.id, name: genre.name });
      });
    })
  }

  publishStory(): void {
    if (this.selectedValue.length === 0) {
      this.errorMessage = "Choose at least one genre!";
      return;
    }

    var age = this.publishstory.controls['agelimit'].value;

    if (+age < 0) {
      this.errorMessage = "Please, enter valid age!";
      return;
    }

    var storys = this.publishstory.getRawValue();
    storys["genreIds"] = this.selectedValue;
    this.service.addStory(storys)
      .subscribe((res: any) => {
        this.router.navigateByUrl('modifystory/' + res.id);
      },
        error => {
          console.error("Publish Error", error)
          let errorstring = "";

          for (const [key, value] of Object.entries(error.error.errors)) {
            errorstring += `${key}: ${value}`;
          }

          if (errorstring.includes("agelimit")) {
            this.errorMessage = "Please, enter valid age!";
            return;
          }
          else {
            alert(errorstring);
          }

          this.errorMessage = error.error;
        });
  }

  cancel(): void {
    this.router.navigateByUrl('');
  }
}
