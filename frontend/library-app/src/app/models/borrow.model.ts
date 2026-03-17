// Represents the BorrowRecord data structure
// Matches the BorrowRecord response from backend API
export interface BorrowRecord {
  borrowId: number;          // Unique borrow ID
  bookId: number;            // Which book was borrowed
  bookTitle: string;         // Book title (from navigation property)
  userName: string;          // Who borrowed (from navigation property)
  borrowDate: Date;          // When borrowed
  returnDate: Date | null;   // When returned — null if not returned yet
  status: string;            // "Borrowed" or "Returned"
}

// Used for Borrow Book API request
export interface BorrowRequest {
  bookId: number;            // Only bookId needed — userId from token
}