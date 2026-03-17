// Represents the User data structure
// Matches the User response from backend API
export interface User {
  userId: number;      // Unique user ID
  name: string;        // Full name of user
  email: string;       // User email
  role: string;        // "Admin" or "User"
}

// Used for Login API request
export interface LoginRequest {
  email: string;
  password: string;
}

// Used for Register API request
export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

// Used for Auth API response (token)
export interface AuthResponse {
  message: string;
  token: string;
}