import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss']
})
export class AuthFormComponent {
  @Input() form?: FormGroup;
  @Input() formTitle?: string;
  @Output() submitForm = new EventEmitter<void>();

  onSubmit(): void {
    console.warn('onSubmit');
    if (this.form?.valid) {
      this.submitForm.emit();
    }
  }
}
