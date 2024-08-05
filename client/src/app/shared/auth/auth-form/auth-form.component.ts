import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss'],
  imports: [FormsModule, ReactiveFormsModule]
})
export class AuthFormComponent {
  @Input() formGroup: FormGroup;
  @Input() formTitle: string;
  @Output() submitForm = new EventEmitter<void>();

  onSubmit(): void {
    if (this.formGroup?.valid) {
      this.submitForm.emit();
    }
  }
}
