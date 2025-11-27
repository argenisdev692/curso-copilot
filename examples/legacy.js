// Legacy JavaScript code for migration example

var userName = 'John';
var tickets = [];
var config = {
  apiUrl: 'https://api.example.com',
  timeout: 5000
};

function createTicket(title, priority, assignedTo) {
  return {
    id: Math.random(),
    title: title,
    priority: priority,
    assignedTo: assignedTo,
    createdAt: new Date()
  };
}

const ticketsArray = [
  { id: 1, title: 'Bug', priority: 'high' },
  { id: 2, title: 'Feature', priority: 'low' }
];

function getHighPriorityTickets(tickets) {
  return tickets.filter(t => t.priority === 'high');
}

function fetchTickets(callback) {
  fetch('/api/tickets')
    .then(response => response.json())
    .then(data => callback(null, data))
    .catch(error => callback(error, null));
}

class TicketService {
  constructor(apiUrl) {
    this.apiUrl = apiUrl;
    this.cache = new Map();
  }

  async getAll() {
    if (this.cache.has('tickets')) {
      return this.cache.get('tickets');
    }

    const tickets = await this.fetchFromApi();
    this.cache.set('tickets', tickets);
    return tickets;
  }

  async fetchFromApi() {
    const response = await fetch(this.apiUrl);
    return response.json();
  }

  clearCache() {
    this.cache.clear();
  }
}

function findTicketById(tickets, id) {
  return tickets.find(t => t.id === id);
}

function findUserById(users, id) {
  return users.find(u => u.id === id);
}

function findCategoryById(categories, id) {
  return categories.find(c => c.id === id);
}

function processResponse(response) {
  if (response.error) {
    console.error(response.error);
    return null;
  }
  return response.data;
}