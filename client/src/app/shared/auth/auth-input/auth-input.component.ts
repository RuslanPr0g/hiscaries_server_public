import { Component, ElementRef, forwardRef, Input, Renderer2 } from '@angular/core';
import { ControlContainer, ControlValueAccessor, FormGroupDirective, NG_VALUE_ACCESSOR } from '@angular/forms';

export const CUSTOM_CONROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => AuthInputComponent),
  multi: true,
};
@Component({
  standalone: true,
  selector: 'app-auth-input',
  templateUrl: './auth-input.component.html',
  styleUrls: ['./auth-input.component.scss'],
  providers: [CUSTOM_CONROL_VALUE_ACCESSOR],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class AuthInputComponent implements ControlValueAccessor {
  @Input({ required: true }) formControlName?: string;
  @Input() type: string = 'text';
  @Input() placeholder?: string;
  value: any;
  onChange: (value: any) => void = () => {};
  onTouched: () => void = () => {};

  writeValue(value: any): void {
    this.value = value;
  }

  registerOnChange(fn: (value: any) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    // Implement if needed
  }

  onInput(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    this.value = inputElement.value;
    this.onChange(this.value);
  }
}
