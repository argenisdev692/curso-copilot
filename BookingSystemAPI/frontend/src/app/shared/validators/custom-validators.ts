import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Custom validators for reactive forms
 */
export class CustomValidators {
  /**
   * Validator for password strength
   * Requires: min 8 chars, uppercase, lowercase, number, special char
   */
  static strongPassword(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return null;
      }

      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasNumeric = /[0-9]/.test(value);
      const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);
      const hasMinLength = value.length >= 8;

      const passwordValid = hasUpperCase && hasLowerCase && hasNumeric && hasSpecialChar && hasMinLength;

      return passwordValid
        ? null
        : {
            strongPassword: {
              hasUpperCase,
              hasLowerCase,
              hasNumeric,
              hasSpecialChar,
              hasMinLength,
            },
          };
    };
  }

  /**
   * Validator to check if two fields match (e.g., password confirmation)
   * @param matchingControlName Name of the control to match against
   */
  static matchField(matchingControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const matchingControl = control.parent?.get(matchingControlName);

      if (!matchingControl) {
        return null;
      }

      if (control.value !== matchingControl.value) {
        return { matchField: true };
      }

      return null;
    };
  }

  /**
   * Group validator para verificar que dos controles de contraseña coincidan.
   * @param passwordKey Nombre del control de contraseña
   * @param confirmPasswordKey Nombre del control de confirmación
   */
  static passwordMatch(passwordKey: string, confirmPasswordKey: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const password = group.get(passwordKey);
      const confirmPassword = group.get(confirmPasswordKey);

      if (!password || !confirmPassword) {
        return null;
      }

      if (password.value !== confirmPassword.value) {
        return { passwordMismatch: true };
      }

      return null;
    };
  }

  /**
   * Validator for date ranges (end date must be after start date)
   * @param startDateControlName Name of the start date control
   */
  static dateAfter(startDateControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const startDateControl = control.parent?.get(startDateControlName);

      if (!startDateControl || !control.value || !startDateControl.value) {
        return null;
      }

      const startDate = new Date(startDateControl.value);
      const endDate = new Date(control.value);

      if (endDate <= startDate) {
        return { dateAfter: true };
      }

      return null;
    };
  }

  /**
   * Validator to check if date is not in the past
   */
  static futureDate(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const selectedDate = new Date(control.value);
      const now = new Date();

      if (selectedDate < now) {
        return { futureDate: true };
      }

      return null;
    };
  }
}
