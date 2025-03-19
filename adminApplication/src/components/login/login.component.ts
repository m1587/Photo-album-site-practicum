import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatToolbarModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  submitted = false;
  errorMessage!: string;
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
  }
  get formControls() {
    return this.loginForm.get('userDetails') as FormGroup;
  }
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      userDetails: this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
      })
    });
  }
  onSubmit() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }
    console.log('Submitted:', this.loginForm.value);
    this.authService.login(
      this.formControls.get('email')?.value,
      this.formControls.get('password')?.value,
    ).subscribe(
      response => {
        alert("login successful 😝😁😊");
        console.log(localStorage);
        this.authService.storeToken(response.token);
        this.authService.storeUserId(response.userId);
        this.authService.storeUserRole(response.role);
        this.router.navigate(['/course']);
      },
      error => {
        this.errorMessage = error.error.message + 'Login error, please try again.';
        alert(this.errorMessage)
        this.router.navigate(['/register']);
      }
    );
  }
}

