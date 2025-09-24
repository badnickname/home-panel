import { Routes } from '@angular/router';
import { MainPageComponent } from './components/main-page/main-page.component';
import { SoundPageComponent } from './components/sound-page/sound-page.component';
import { TimerPageComponent } from './components/timer-page/timer-page.component';
import { VoicePageComponent } from './components/voice-page/voice-page.component';

export const routes: Routes = [
  {
    path: '',
    component: MainPageComponent,
  },
  {
    path: 'sound',
    component: SoundPageComponent,
  },
  {
    path: 'timer',
    component: TimerPageComponent,
  },
  {
    path: 'voice',
    component: VoicePageComponent,
  },
];
