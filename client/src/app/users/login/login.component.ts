import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthButtonComponent } from '../../shared/auth/auth-button/auth-button.component';
import { AuthFormComponent } from '../../shared/auth/auth-form/auth-form.component';
import { AuthInputComponent } from '../../shared/auth/auth-input/auth-input.component';
import { UserService } from '../services/user.service';

@Component({
  standalone: true,
  imports: [AuthButtonComponent, AuthInputComponent, AuthFormComponent, FormsModule, ReactiveFormsModule],
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  formlogin: FormGroup;
  formregister: FormGroup;
  isLoginState: boolean = true;
  errorMessage: string = '';

  constructor(
    private service: UserService,
    private router: Router) {
    this.formlogin = new FormGroup({
      Username: new FormControl('', Validators.required),
      Password: new FormControl('', Validators.required)
    });

    this.formregister = new FormGroup({
      Username: new FormControl('', [Validators.required, Validators.minLength(3)]),
      Password: new FormControl('', [Validators.required, Validators.minLength(3)]),
      Email: new FormControl('', [Validators.required, Validators.email]),
      Dob: new FormControl('', Validators.required)
    });
  }

  ngOnInit(): void {
    if (this.service.isAuthenticated()) {
      this.router.navigateByUrl('');
    }
  }

  changeState(): void {
    this.isLoginState = !this.isLoginState;
    this.errorMessage = '';
  }

  logIn(): void {
    console.warn(this.formlogin?.value);
    return;

    if (this.formlogin?.invalid) {
      this.errorMessage = "All fields are required!";
      return;
    }

    this.service.login(this.formlogin?.value).subscribe(
      {
        next: () => this.router.navigateByUrl(''),
        error: error => this.handleError(error)
      }
    );
  }

  signUp(): void {
    console.warn(this.formregister?.value);
    return;

    if (this.formregister?.invalid) {
      this.errorMessage = "All fields are required and must be valid!";
      return;
    }

    this.service.register(this.formregister?.value).subscribe(
      {
        next: () => this.router.navigateByUrl(''),
        error: error => this.handleError(error)
      }
    );
  }

  private handleError(error: any): void {
    console.error("Error", error);
    this.errorMessage = error.error?.title || error.error || 'An unexpected error occurred';
  }
}
