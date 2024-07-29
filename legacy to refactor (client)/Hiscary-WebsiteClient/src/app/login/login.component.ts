import { Component, OnInit } from '@angular/core';
import { fakeAsync } from '@angular/core/testing';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TestsharedService } from '../testshared.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  formlogin: FormGroup;
  formregister: FormGroup;
  isLoginState: boolean;
  errorMessage: string = '';

  constructor(private formBuilder: FormBuilder, private service: TestsharedService, private router: Router) {
    this.isLoginState = true;

    this.formlogin = this.formBuilder.group({
      username: '',
      password: ''
    })

    this.formregister = this.formBuilder.group({
      username: '',
      password: '',
      email: undefined,
      dob: '2022-01-02'
    })

    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }

  ngOnInit(): void {
    if (this.service.isAuthenticated()) {
      this.router.navigateByUrl('');
    }
  }

  changeState(state: boolean): void {
    this.isLoginState = state;
  }

  logIn(): void {
    this.service.login(this.formlogin.getRawValue())
      .subscribe(res => {
        this.router.navigateByUrl('');
      },
        error => {
          console.error("Login Error", error)
          this.errorMessage = error.error;
        });
  }

  validateEmail(email: string) {
    return String(email)
      .toLowerCase()
      .match(
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      );
  };

  signUp(): void {
    var email = this.formregister.getRawValue().email;
    var username = this.formregister.getRawValue().username;
    var password = this.formregister.getRawValue().password;
    
    if (!this.validateEmail(email)) {
      this.errorMessage = "It's not an email!";
      return;
    }
    else {
      this.errorMessage = '';
    }

    if (username.legth < 3 || password.legth < 3)
    {
      alert("Username and Password must be at least 3 characters!");
    }

    this.service.register(this.formregister.getRawValue())
      .subscribe(res => {
        this.router.navigateByUrl('');
      },
        error => {
          console.error("Register Error", error)

          if (!error.error.title) {
            this.errorMessage = error.error;
            return;
          }

          var errors = '';

          if (error.error.errors.Username) {
            errors += error.error.errors.Username[0] || '';
          }

          if (error.error.errors.Password) {
            if (error.error.errors.Username) {
              errors += ', ';
            }
            errors += error.error.errors.Password[0] || '';
          }

          this.errorMessage = errors;
        });
  }
}
