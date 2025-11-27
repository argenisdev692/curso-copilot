import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { TicketService, GetTicketsParams, PagedResponse } from './ticket.service';
import { Ticket } from '../../shared/models/common/ticket.interface';

describe('TicketService', () => {
  let service: TicketService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TicketService]
    });
    service = TestBed.inject(TicketService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getTickets', () => {
    it('should make a GET request to /api/tickets with no params', () => {
      const mockResponse: PagedResponse<Ticket> = {
        items: [],
        totalItems: 0,
        page: 1,
        pageSize: 10,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getTickets().subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne('/api/tickets');
      expect(req.request.method).toBe('GET');
      expect(req.request.params.keys().length).toBe(0);
      req.flush(mockResponse);
    });

    it('should make a GET request with all params', () => {
      const params: GetTicketsParams = {
        page: 2,
        pageSize: 20,
        status: 'Open',
        priority: 'High',
        assignedTo: 123,
        search: 'test search',
        sortBy: 'title',
        sortOrder: 'asc'
      };

      const mockResponse: PagedResponse<Ticket> = {
        items: [],
        totalItems: 0,
        page: 2,
        pageSize: 20,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getTickets(params).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(req => req.url === '/api/tickets');
      expect(req.request.method).toBe('GET');
      expect(req.request.params.get('page')).toBe('2');
      expect(req.request.params.get('pageSize')).toBe('20');
      expect(req.request.params.get('status')).toBe('Open');
      expect(req.request.params.get('priority')).toBe('High');
      expect(req.request.params.get('assignedTo')).toBe('123');
      expect(req.request.params.get('search')).toBe('test search');
      expect(req.request.params.get('sortBy')).toBe('title');
      expect(req.request.params.get('sortOrder')).toBe('asc');
      req.flush(mockResponse);
    });

    it('should make a GET request with partial params', () => {
      const params: GetTicketsParams = {
        page: 1,
        status: 'Closed'
      };

      const mockResponse: PagedResponse<Ticket> = {
        items: [],
        totalItems: 0,
        page: 1,
        pageSize: 10,
        totalPages: 0,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getTickets(params).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(req => req.url === '/api/tickets');
      expect(req.request.method).toBe('GET');
      expect(req.request.params.get('page')).toBe('1');
      expect(req.request.params.get('status')).toBe('Closed');
      expect(req.request.params.get('pageSize')).toBeNull();
      req.flush(mockResponse);
    });

    it('should handle HTTP error', () => {
      service.getTickets().subscribe(
        () => fail('should have failed'),
        error => expect(error.status).toBe(500)
      );

      const req = httpMock.expectOne('/api/tickets');
      req.flush('Server error', { status: 500, statusText: 'Server Error' });
    });
  });

  // Note: Currently, only getTickets is implemented. If more endpoints are added,
  // add corresponding test cases here.
});
