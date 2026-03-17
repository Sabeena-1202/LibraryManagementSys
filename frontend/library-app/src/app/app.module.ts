import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Interceptor
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';

// Shared Components
import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';

// Pages
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { BookListComponent } from './pages/book-list/book-list.component';
import { BorrowBookComponent } from './pages/borrow-book/borrow-book.component';
import { AdminDashboardComponent } from './pages/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './pages/user-dashboard/user-dashboard.component';

@NgModule({
  declarations: [
    AppComponent,

    // Shared
    NavbarComponent,
    FooterComponent,

    // Pages
    LoginComponent,
    RegisterComponent,
    BookListComponent,
    BorrowBookComponent,
    AdminDashboardComponent,
    UserDashboardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    // Register JWT Interceptor
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }