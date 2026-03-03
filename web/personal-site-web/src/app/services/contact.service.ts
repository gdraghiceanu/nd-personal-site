import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ContactRequest {
  name: string;
  email: string;
  message: string;
}

export interface ContactResponse {
  message: string;
}

@Injectable({ providedIn: 'root' })
export class ContactService {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiBaseUrl}/api/contact`;

  send(request: ContactRequest): Observable<ContactResponse> {
    return this.http.post<ContactResponse>(this.url, request);
  }
}
