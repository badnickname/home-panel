import { Component } from '@angular/core';
import { SoundPageComponent } from '../sound-page/sound-page.component';
import { TimerPageComponent } from '../timer-page/timer-page.component';
import { VoicePageComponent } from '../voice-page/voice-page.component';

@Component({
  selector: 'app-main-page',
  standalone: true,
  templateUrl: './main-page.component.html',
  imports: [SoundPageComponent, TimerPageComponent, VoicePageComponent],
  styleUrl: './main-page.component.css',
})
export class MainPageComponent {}
