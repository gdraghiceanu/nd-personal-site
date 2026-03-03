import { Component } from '@angular/core';
import { HeroSectionComponent } from '../../components/hero-section/hero-section.component';
import { AboutSectionComponent } from '../../components/about-section/about-section.component';
import { PhotoSectionComponent } from '../../components/photo-section/photo-section.component';
import { ContactCtaComponent } from '../../components/contact-cta/contact-cta.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [
    HeroSectionComponent,
    AboutSectionComponent,
    PhotoSectionComponent,
    ContactCtaComponent
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {}
