export class CustomValidator {
  // Validates numbers
  static numberValidator(number: any): any {
    if (number.pristine) {
      return null;
    }
    // const NUMBER_REGEXP = /^-?[\d.]+(?:e-?\d+)?$/; // number with decimal, negative
    const NUMBER_REGEXP = /^[0-9]*$/; // onlynumber

    number.markAsTouched();
    if (NUMBER_REGEXP.test(number.value)) {
      return null;
    }
    return {
      invalidNumber: true,
    };
  }
}
