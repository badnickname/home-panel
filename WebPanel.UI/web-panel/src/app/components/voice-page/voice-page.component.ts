import { Component, computed, inject } from '@angular/core';
import { Settings } from '../../services/settings';
import { MatSlideToggle, MatSlideToggleChange } from '@angular/material/slide-toggle';
import { Voice } from '../../models/voice';

@Component({
  selector: 'app-voice-page',
  standalone: true,
  templateUrl: './voice-page.component.html',
  imports: [MatSlideToggle],
  styleUrl: './voice-page.component.css',
})
export class VoicePageComponent {
  private settings = inject(Settings);
  enabled = computed(() => this.settings.voice()?.listen);
  disabled = computed(() => !this.settings.voice()?.listen);
  setEnabled(change: MatSlideToggleChange): void {
    this.settings.setVoice(new Voice(change.checked));
  }
}
