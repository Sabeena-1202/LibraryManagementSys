import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  // App title
  title = 'Library Management System';

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
}