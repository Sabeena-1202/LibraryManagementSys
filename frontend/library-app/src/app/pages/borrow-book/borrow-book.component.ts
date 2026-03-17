import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../core/services/book.service';
import { BorrowService } from '../../core/services/borrow.service';
import { Book } from '../../models/book.model';
import { BorrowRecord } from '../../models/borrow.model';

@Component({
  selector: 'app-borrow-book',
  templateUrl: './borrow-book.component.html',
  styleUrls: ['./borrow-book.component.css']
})
export class BorrowBookComponent implements OnInit {

  // Selected book details
  selectedBook: Book | null = null;

  // User borrow history
  borrowHistory: BorrowRecord[] = [];

  // UI state
  isLoading: boolean = false;
  isBorrowing: boolean = false;
  isReturning: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  bookId: number = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private borrowService: BorrowService
  ) {}

  ngOnInit(): void {
    // Get bookId from query params
    this.route.queryParams.subscribe(params => {
      if (params['bookId']) {
        this.bookId = parseInt(params['bookId']);
        this.loadBookDetails(this.bookId);
      }
    });

    // Load borrow history
    this.loadBorrowHistory();
  }

  // Load book details
  loadBookDetails(id: number): void {
    this.isLoading = true;
    this.bookService.getBookById(id).subscribe({
      next: (book) => {
        this.selectedBook = book;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load book details';
      }
    });
  }

  // Load borrow history
  loadBorrowHistory(): void {
    this.borrowService.getMyBorrowHistory().subscribe({
      next: (records) => {
        this.borrowHistory = records;
      },
      error: (error) => {
        console.error('Error loading borrow history', error);
      }
    });
  }

  // Borrow book
  borrowBook(): void {
    if (!this.selectedBook) return;

    this.isBorrowing = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.borrowService.borrowBook({ bookId: this.selectedBook.bookId })
      .subscribe({
        next: (response) => {
          this.isBorrowing = false;
          this.successMessage = 'Book borrowed successfully!';

          // Reload book details and history
          this.loadBookDetails(this.selectedBook!.bookId);
          this.loadBorrowHistory();
        },
        error: (error) => {
          this.isBorrowing = false;
          this.errorMessage = error.error?.message || 'Failed to borrow book';
        }
      });
  }

  // Return book
  returnBook(bookId: number): void {
    this.isReturning = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.borrowService.returnBook(bookId).subscribe({
      next: (response) => {
        this.isReturning = false;
        this.successMessage = 'Book returned successfully!';

        // Reload data
        if (this.selectedBook) {
          this.loadBookDetails(this.selectedBook.bookId);
        }
        this.loadBorrowHistory();
      },
      error: (error) => {
        this.isReturning = false;
        this.errorMessage = error.error?.message || 'Failed to return book';
      }
    });
  }

  // Get badge color based on status
  getStatusBadge(status: string): string {
    return status === 'Borrowed' ? 'bg-warning text-dark' : 'bg-success';
  }

  // Go back to books
  goBack(): void {
    this.router.navigate(['/books']);
  }
}