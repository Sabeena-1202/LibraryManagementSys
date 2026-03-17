import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { RegisterRequest } from '../../models/user.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  // Form model
  registerData: RegisterRequest = {
    name: '',
    email: '',
    password: ''
  };

  // UI state
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  showPassword: boolean = false;
  confirmPassword: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  // Toggle password visibility
  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  // Register form submit
  onRegister(): void {
    // Clear previous messages
    this.errorMessage = '';
    this.successMessage = '';

    // Validate form
    if (!this.registerData.name ||
        !this.registerData.email ||
        !this.registerData.password) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    // Validate password length
    if (this.registerData.password.length < 6) {
      this.errorMessage = 'Password must be at least 6 characters';
      return;
    }

    // Validate confirm password
    if (this.registerData.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    // Show loading
    this.isLoading = true;

    // Call register API
    this.authService.register(this.registerData).subscribe({
      next: (response) => {
        // Save token
        this.authService.saveToken(response.token);

        this.isLoading = false;
        this.successMessage = 'Registration successful! Redirecting...';

        // Redirect to user dashboard
        setTimeout(() => {
          this.router.navigate(['/user-dashboard']);
        }, 1500);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Registration failed. Please try again.';
      }
    });
  }
}