// Modernized JavaScript code - migrated from legacy.js

const userName = 'John';
const tickets = [];
const config = {
  apiUrl: 'https://api.example.com',
  timeout: 5000
};

const createTicket = (title, priority, assignedTo) => ({
  id: Math.random(),
  title,
  priority,
  assignedTo,
  createdAt: new Date()
});

const ticketsArray = [
  { id: 1, title: 'Bug', priority: 'high' },
  { id: 2, title: 'Feature', priority: 'low' }
];

const getHighPriorityTickets = (tickets) => tickets.filter(ticket => ticket.priority === 'high');

const fetchTickets = async () => {
  try {
    const response = await fetch('/api/tickets');
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error fetching tickets:', error);
    throw error;
  }
};

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
    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }
    return response.json();
  }

  clearCache() {
    this.cache.clear();
  }
}

const findTicketById = (tickets, id) => tickets.find(ticket => ticket.id === id);

const findUserById = (users, id) => users.find(user => user.id === id);

const findCategoryById = (categories, id) => categories.find(category => category.id === id);

const processResponse = (response) => {
  if (response.error) {
    console.error(response.error);
    return null;
  }
  return response.data;
};