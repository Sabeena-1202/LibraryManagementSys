// Represents the Book data structure
// Matches the Book response from backend API
export interface Book {
  bookId: number;              // Unique book ID
  title: string;               // Book title
  author: string;              // Book author
  category: string;            // Book category (Fiction, Science etc.)
  publishedYear: number;       // Year published
  availabilityStatus: string;  // "Available" or "Borrowed"
}

// Used for Add Book API request (Admin only)
export interface BookCreateRequest {
  title: string;
  author: string;
  category: string;
  publishedYear: number;
}

// Used for Update Book API request (Admin only)
export interface BookUpdateRequest {
  title?: string;              // Optional — update only if provided
  author?: string;
  category?: string;
  publishedYear?: number;
  availabilityStatus?: string;
}