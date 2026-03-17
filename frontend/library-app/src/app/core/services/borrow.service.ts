import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BorrowRecord, BorrowRequest } from '../../models/borrow.model';

@Injectable({
  providedIn: 'root'
})
export class BorrowService {

  // Backend API base URL
  private apiUrl = 'https://localhost:7181/api/borrow';

  constructor(private http: HttpClient) {}

  // Borrow a book — User only
  borrowBook(request: BorrowRequest): Observable<any> {
    return this.http.post<any>(this.apiUrl, request);
  }

  // Return a book — User only
  returnBook(bookId: number): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/return/${bookId}`, {});
  }

  // Get my borrow history — User only
  getMyBorrowHistory(): Observable<BorrowRecord[]> {
    return this.http.get<BorrowRecord[]>(`${this.apiUrl}/history`);
  }

  // Get all borrow records — Admin only
  getAllBorrowRecords(): Observable<BorrowRecord[]> {
    return this.http.get<BorrowRecord[]>(`${this.apiUrl}/all`);
  }
}
