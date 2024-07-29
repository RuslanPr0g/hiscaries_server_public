import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TestsharedService } from '../testshared.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private service: TestsharedService, private router: Router) {
    this.formuserdata = this.formBuilder.group({
      email: '',
      dob: ''
    })

    this.formupdatepassword = this.formBuilder.group({
      oldPassword: '',
      newPassword: '',
    })
  }

  formuserdata: FormGroup;
  formupdatepassword: FormGroup;

  User: any = {};
  IsError: boolean = false;
  IsLoading: boolean = true;
  errorMessage: string = '';

  StoryList: any = []

  ngOnInit(): void {
    if (this.service.isAuthenticated() === false) {
      this.router.navigateByUrl('login');
    }

    this.getUserInfo();
    this.refreshStoryList();
  }

  getUserInfo(): any {
    this.service.getUserInfo().subscribe(data => {
      this.User = data;
      let isError = this.User == null || this.User == undefined ? true : false;

      this.formuserdata.controls['email'].setValue(data.email);
      this.formuserdata.controls['dob'].setValue(formatDate(data.birthDate,'yyyy-MM-dd','en'));
      
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

  updatePassword(): void {
    this.service.updateUsersData({
      previousPassword: this.formupdatepassword.getRawValue().oldPassword,
      newPassword: this.formupdatepassword.getRawValue().newPassword
    }).subscribe(
      data => {
        this.service.logOut();
        this.router.navigateByUrl('login');
      },
      error => {
        console.log(error)
        this.errorMessage = error.error;
      });
  }

  refreshStoryList() {
    this.service.getStoryList().subscribe(data => {
      this.StoryList = data.filter(item => item.publisherId == this.User.id).reverse();
    })
  }

  validateEmail(email: string) {
    return String(email)
      .toLowerCase()
      .match(
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      );
  };

  updateUserData(): void {
    var email = this.formuserdata.getRawValue().email;

    if (!this.validateEmail(email)) {
      this.errorMessage = "It's not an email!";
      return;
    }
    else {
      this.errorMessage = '';
    }

    this.service.updateUsersData({
      email: email,
      birthDate: this.formuserdata.getRawValue().dob
    }).subscribe(
      data => {
        this.getUserInfo();
      },
      error => {
        console.log(error)
        this.errorMessage = error.error;

        setTimeout(()=> {
          this.errorMessage = '';
        }, 6000);
      });
  }
}
