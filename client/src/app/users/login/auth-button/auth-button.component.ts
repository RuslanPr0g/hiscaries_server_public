import { Component, Input } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-auth-button',
  templateUrl: './auth-button.component.html',
  styleUrls: ['./auth-button.component.scss']
})
export class AuthButtonComponent {
  @Input() type: string = 'button';
}
