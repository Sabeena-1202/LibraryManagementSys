import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { BookService } from '../../core/services/book.service';
import { BorrowService } from '../../core/services/borrow.service';
import { Book, BookCreateRequest, BookUpdateRequest } from '../../models/book.model';
import { BorrowRecord } from '../../models/borrow.model';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {

  // Admin name
  adminName: string = '';

  // Books list
  books: Book[] = [];

  // Borrow records
  borrowRecords: BorrowRecord[] = [];

  // Dashboard stats
  totalBooks: number = 0;
  availableBooks: number = 0;
  borrowedBooks: number = 0;
  totalBorrowRecords: number = 0;

  // Active tab
  activeTab: string = 'books';

  // Add book form
  newBook: BookCreateRequest = {
    title: '',
    author: '',
    category: '',
    publishedYear: new Date().getFullYear()
  };

  // Edit book form
  editBook: BookUpdateRequest & { bookId?: number } = {};
  isEditing: boolean = false;

  // UI state
  isLoading: boolean = false;
  isSubmitting: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private authService: AuthService,
    private bookService: BookService,
    private borrowService: BorrowService
  ) {}

  ngOnInit(): void {
    // Get admin name from token
    this.adminName = this.authService.getUserName();

    // Load data
    this.loadBooks();
    this.loadBorrowRecords();
  }

  // =====================
  // LOAD DATA
  // =====================

  // Load all books
  loadBooks(): void {
    this.isLoading = true;
    this.bookService.getAllBooks().subscribe({
      next: (books) => {
        this.books = books;

        // Calculate stats
        this.totalBooks = books.length;
        this.availableBooks = books
          .filter(b => b.availabilityStatus === 'Available').length;
        this.borrowedBooks = books
          .filter(b => b.availabilityStatus === 'Borrowed').length;

        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load books';
      }
    });
  }

  // Load all borrow records
  loadBorrowRecords(): void {
    this.borrowService.getAllBorrowRecords().subscribe({
      next: (records) => {
        this.borrowRecords = records;
        this.totalBorrowRecords = records.length;
      },
      error: (error) => {
        console.error('Error loading borrow records', error);
      }
    });
  }

  // =====================
  // BOOK CRUD
  // =====================

  // Add new book
  addBook(): void {
    if (!this.newBook.title || !this.newBook.author ||
        !this.newBook.category || !this.newBook.publishedYear) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.bookService.addBook(this.newBook).subscribe({
      next: (book) => {
        this.isSubmitting = false;
        this.successMessage = `"${book.title}" added successfully!`;

        // Reset form
        this.newBook = {
          title: '',
          author: '',
          category: '',
          publishedYear: new Date().getFullYear()
        };

        // Reload books
        this.loadBooks();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to add book';
      }
    });
  }

  // Set book for editing
  setEditBook(book: Book): void {
    this.isEditing = true;
    this.editBook = {
      bookId: book.bookId,
      title: book.title,
      author: book.author,
      category: book.category,
      publishedYear: book.publishedYear,
      availabilityStatus: book.availabilityStatus
    };

    // Switch to books tab
    this.activeTab = 'books';

    // Scroll to top
    window.scrollTo(0, 0);
  }

  // Update book
  updateBook(): void {
    if (!this.editBook.bookId) return;

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.bookService.updateBook(this.editBook.bookId, this.editBook)
      .subscribe({
        next: (book) => {
          this.isSubmitting = false;
          this.successMessage = `"${book.title}" updated successfully!`;
          this.isEditing = false;
          this.editBook = {};
          this.loadBooks();
        },
        error: (error) => {
          this.isSubmitting = false;
          this.errorMessage = 'Failed to update book';
        }
      });
  }

  // Cancel edit
  cancelEdit(): void {
    this.isEditing = false;
    this.editBook = {};
    this.errorMessage = '';
  }

  // Delete book
  deleteBook(id: number, title: string): void {
    if (!confirm(`Are you sure you want to delete "${title}"?`)) return;

    this.errorMessage = '';
    this.successMessage = '';

    this.bookService.deleteBook(id).subscribe({
      next: () => {
        this.successMessage = `"${title}" deleted successfully!`;
        this.loadBooks();
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to delete book';
      }
    });
  }

  // =====================
  // UI HELPERS
  // =====================

  // Switch tab
  setActiveTab(tab: string): void {
    this.activeTab = tab;
    this.errorMessage = '';
    this.successMessage = '';
  }

  // Get badge color
  getStatusBadge(status: string): string {
    return status === 'Available' ? 'bg-success' : 'bg-danger';
  }

  // Get borrow status badge
  getBorrowBadge(status: string): string {
    return status === 'Borrowed' ? 'bg-warning text-dark' : 'bg-success';
  }
}