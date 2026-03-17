import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookService } from '../../core/services/book.service';
import { AuthService } from '../../core/services/auth.service';
import { Book } from '../../models/book.model';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {

  // All books list
  books: Book[] = [];

  // Filtered books list
  filteredBooks: Book[] = [];

  // UI state
  isLoading: boolean = false;
  errorMessage: string = '';
  searchTerm: string = '';

  constructor(
    private bookService: BookService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Load all books on page load
    this.loadBooks();
  }

  // Load all books from API
  loadBooks(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.bookService.getAllBooks().subscribe({
      next: (books) => {
        this.books = books;
        this.filteredBooks = books;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load books. Please try again.';
      }
    });
  }

  // Search books by title or author
  onSearch(): void {
    if (!this.searchTerm.trim()) {
      // If search is empty → show all books
      this.filteredBooks = this.books;
      return;
    }

    // Filter books locally
    this.filteredBooks = this.books.filter(book =>
      book.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      book.author.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  // Clear search
  clearSearch(): void {
    this.searchTerm = '';
    this.filteredBooks = this.books;
  }

  // Check if user is logged in
  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  // Check if user is Admin
  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  // Borrow book — redirect to borrow page
  borrowBook(bookId: number): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.router.navigate(['/borrow'], { queryParams: { bookId } });
  }

  // Get badge color based on availability
  getStatusBadge(status: string): string {
    return status === 'Available' ? 'bg-success' : 'bg-danger';
  }
}