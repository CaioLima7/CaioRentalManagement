import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/auth/login`, { username, password }).pipe(
      tap((response: any) => {
        localStorage.setItem('auth_token', response.token);
      })
    );
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('auth_token');
    // Implement token validation logic here
    return !!token;
  }

  logout() {
    localStorage.removeItem('auth_token');
  }
}
