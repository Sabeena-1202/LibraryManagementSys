import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Guards
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';

// Pages
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { BookListComponent } from './pages/book-list/book-list.component';
import { BorrowBookComponent } from './pages/borrow-book/borrow-book.component';
import { AdminDashboardComponent } from './pages/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './pages/user-dashboard/user-dashboard.component';

const routes: Routes = [

  // Default route → redirect to book list
 // Default route → redirect to login
{ path: '', redirectTo: '/login', pathMatch: 'full' },

  // Public routes — no guard needed
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'books', component: BookListComponent },

  // User routes — must be logged in
  {
    path: 'borrow',
    component: BorrowBookComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'user-dashboard',
    component: UserDashboardComponent,
    canActivate: [AuthGuard]
  },

  // Admin routes — must be Admin role
  {
    path: 'admin-dashboard',
    component: AdminDashboardComponent,
    canActivate: [AdminGuard]
  },

  // Unknown routes → redirect to books
  { path: '**', redirectTo: '/books' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
