import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book, BookCreateRequest, BookUpdateRequest } from '../../models/book.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  // Backend API base URL
  private apiUrl = 'https://localhost:7181/api/book';

  constructor(private http: HttpClient) {}

  // Get all books — Public
  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl);
  }

  // Get book by id — Public
  getBookById(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }

  // Search books by title or author — Public
  searchBooks(searchTerm: string): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.apiUrl}/search?searchTerm=${searchTerm}`);
  }

  // Add new book — Admin only
  addBook(book: BookCreateRequest): Observable<Book> {
    return this.http.post<Book>(this.apiUrl, book);
  }

  // Update book — Admin only
  updateBook(id: number, book: BookUpdateRequest): Observable<Book> {
    return this.http.put<Book>(`${this.apiUrl}/${id}`, book);
  }

  // Delete book — Admin only
  deleteBook(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}