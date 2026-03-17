import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  // Check if user is logged in
  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  // Check if user is Admin
  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  // Get logged in user name
  getUserName(): string {
    return this.authService.getUserName();
  }

  // Logout — clear token and redirect to login
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}