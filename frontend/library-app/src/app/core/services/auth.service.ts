import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // Backend API base URL
  private apiUrl = 'https://localhost:7181/api/auth';

  constructor(private http: HttpClient) {}

  // Register new user
  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request);
  }

  // Login user
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request);
  }

  // Save token to localStorage after login
  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  // Get token from localStorage
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // Remove token on logout
  logout(): void {
    localStorage.removeItem('token');
  }

  // Check if user is logged in
  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  // Get user role from token
  getUserRole(): string {
    const token = this.getToken();
    if (!token) return '';

    // Decode JWT token payload
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  }

  // Get user name from token
  getUserName(): string {
    const token = this.getToken();
    if (!token) return '';

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
  }

  // Get user id from token
  getUserId(): number {
    const token = this.getToken();
    if (!token) return 0;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return parseInt(payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']);
  }

  // Check if user is Admin
  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }

  // Check if user is normal User
  isUser(): boolean {
    return this.getUserRole() === 'User';
  }
}