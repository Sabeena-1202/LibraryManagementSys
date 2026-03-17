import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { LoginRequest } from '../../models/user.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  // Form model
  loginData: LoginRequest = {
    email: '',
    password: ''
  };

  // UI state
  isLoading: boolean = false;
  errorMessage: string = '';
  showPassword: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  // Toggle password visibility
  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  // Login form submit
  onLogin(): void {
    // Clear previous errors
    this.errorMessage = '';

    // Validate form
    if (!this.loginData.email || !this.loginData.password) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    // Show loading
    this.isLoading = true;

    // Call login API
    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        // Save token
        this.authService.saveToken(response.token);

        this.isLoading = false;

        // Redirect based on role
        if (this.authService.isAdmin()) {
          this.router.navigate(['/admin-dashboard']);
        } else {
          this.router.navigate(['/user-dashboard']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Login failed. Please try again.';
      }
    });
  }
}