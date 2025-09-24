import { Component, computed, inject } from '@angular/core';
import { Settings } from '../../services/settings';
import { MatInput, MatFormField } from '@angular/material/input';
import { Timer } from '../../models/timer';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-timer-page',
  standalone: true,
  templateUrl: './timer-page.component.html',
  imports: [MatFormField, MatInput, MatButton],
  styleUrl: './timer-page.component.css',
})
export class TimerPageComponent {
  private settings = inject(Settings);
  timer = computed(() => this.settings.timer()?.seconds ?? 0);
  setTimer(event: Event): void {
    this.settings.setTimer(new Timer(Number((event.target as any).value)));
  }
  setHour(hour: number): void {
    this.settings.setTimer(new Timer(hour * 3600));
  }
}
