import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { BorrowService } from '../../core/services/borrow.service';
import { BorrowRecord } from '../../models/borrow.model';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.css']
})
export class UserDashboardComponent implements OnInit {

  // User info
  userName: string = '';

  // Borrow history
  borrowHistory: BorrowRecord[] = [];

  // Dashboard stats
  totalBorrowed: number = 0;
  currentlyBorrowed: number = 0;
  totalReturned: number = 0;

  // UI state
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  isReturning: boolean = false;

  constructor(
    private authService: AuthService,
    private borrowService: BorrowService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Get user name from token
    this.userName = this.authService.getUserName();

    // Load borrow history
    this.loadBorrowHistory();
  }

  // Load borrow history
  loadBorrowHistory(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.borrowService.getMyBorrowHistory().subscribe({
      next: (records) => {
        this.borrowHistory = records;

        // Calculate stats
        this.totalBorrowed = records.length;
        this.currentlyBorrowed = records
          .filter(r => r.status === 'Borrowed').length;
        this.totalReturned = records
          .filter(r => r.status === 'Returned').length;

        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load borrow history';
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

  // Go to book list
  browseBooks(): void {
    this.router.navigate(['/books']);
  }

  // Logout
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}