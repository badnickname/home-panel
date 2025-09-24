import { Component, computed, inject } from '@angular/core';
import { Settings } from '../../services/settings';
import { MatSlider, MatSliderThumb } from '@angular/material/slider';
import { Volume } from '../../models/volume';

@Component({
  selector: 'app-sound-page',
  standalone: true,
  templateUrl: './sound-page.component.html',
  imports: [MatSlider, MatSliderThumb],
  styleUrl: './sound-page.component.css',
})
export class SoundPageComponent {
  private settings = inject(Settings);
  volume = computed(() => this.settings.volume()?.level ?? 0);
  setVolume(volume: number): void {
    this.settings.setVolume(new Volume(volume));
  }
}
