import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ContactService } from '../../services/contact.service';
import { environment } from '../../../environments/environment';
import { finalize } from 'rxjs';

type FormState = 'idle' | 'loading' | 'success' | 'error';

@Component({
  selector: 'app-contact-cta',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './contact-cta.component.html',
  styleUrl: './contact-cta.component.scss'
})
export class ContactCtaComponent {
  private readonly fb = inject(FormBuilder);
  private readonly contactService = inject(ContactService);

  readonly contactEmail = environment.contactEmail;

  state: FormState = 'idle';

  form = this.fb.group({
    name:    ['', [Validators.required, Validators.maxLength(120)]],
    email:   ['', [Validators.required, Validators.email, Validators.maxLength(254)]],
    message: ['', [Validators.required, Validators.maxLength(2000)]]
  });

  get name()    { return this.form.controls.name; }
  get email()   { return this.form.controls.email; }
  get message() { return this.form.controls.message; }

  onSubmit(): void {
    if (this.form.invalid || this.state === 'loading') return;

    this.state = 'loading';

    this.contactService.send(this.form.getRawValue() as any)
      .pipe(finalize(() => {
        if (this.state === 'loading') this.state = 'error';
      }))
      .subscribe({
        next: () => {
          this.state = 'success';
          this.form.reset();
        },
        error: () => {
          this.state = 'error';
        }
      });
  }

  retry(): void {
    this.state = 'idle';
  }
}
