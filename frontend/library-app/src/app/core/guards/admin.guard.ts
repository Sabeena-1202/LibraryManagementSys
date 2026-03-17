import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {

    // Check if user is logged in
    if (!this.authService.isLoggedIn()) {
      // Not logged in → redirect to login
      this.router.navigate(['/login']);
      return false;
    }

    // Check if user is Admin
    if (this.authService.isAdmin()) {
      return true; // Allow access
    }

    // Logged in but not Admin → redirect to user dashboard
    this.router.navigate(['/user-dashboard']);
    return false;
  }
}