import { Pipe, PipeTransform } from '@angular/core';

/**
 * Pipe for truncating text with ellipsis
 */
@Pipe({
  name: 'truncate',
  standalone: true,
})
export class TruncatePipe implements PipeTransform {
  /**
   * Truncate text to specified length
   * @param value Text to truncate
   * @param limit Maximum length (default: 100)
   * @param ellipsis Ellipsis string (default: '...')
   * @returns Truncated text
   */
  transform(value: string | null | undefined, limit: number = 100, ellipsis: string = '...'): string {
    if (!value) return '';

    if (value.length <= limit) {
      return value;
    }

    return value.substring(0, limit).trim() + ellipsis;
  }
}
