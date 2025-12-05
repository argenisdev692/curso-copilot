import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

/**
 * Pipe for formatting dates in a consistent format
 */
@Pipe({
  name: 'appDate',
  standalone: true,
})
export class AppDatePipe implements PipeTransform {
  private datePipe = new DatePipe('en-US');

  /**
   * Transform date to specified format
   * @param value Date value
   * @param format Format string (default: 'medium')
   * @returns Formatted date string
   */
  transform(value: Date | string | null | undefined, format: string = 'medium'): string {
    if (!value) return '';

    const formatMap: Record<string, string> = {
      short: 'MM/dd/yyyy',
      medium: 'MMM dd, yyyy',
      long: 'MMMM dd, yyyy',
      full: 'EEEE, MMMM dd, yyyy',
      time: 'HH:mm',
      datetime: 'MMM dd, yyyy HH:mm',
    };

    const dateFormat = formatMap[format] || format;
    return this.datePipe.transform(value, dateFormat) || '';
  }
}
