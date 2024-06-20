import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginData = {
    username: '',
    password: ''
  };

  constructor(private authService: AuthService, private router: Router) { }

  login() {
    this.authService.login(this.loginData.username, this.loginData.password).subscribe(
      success => {
        if (success) {
          this.router.navigate(['/home']);
        } else {
          alert('Login failed');
        }
      }
    );
  }
}
