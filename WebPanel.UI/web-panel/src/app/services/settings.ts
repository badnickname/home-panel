import { Injectable, signal, inject, Signal, computed, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Volume } from '../models/volume';
import { rxResource } from '@angular/core/rxjs-interop';
import { Timer } from '../models/timer';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Voice } from '../models/voice';
import { interval, Subject, takeUntil } from 'rxjs';

@Injectable()
export abstract class Settings {
  abstract volume: Signal<Volume | undefined>;
  abstract timer: Signal<Timer | undefined>;
  abstract voice: Signal<Voice | undefined>;
  abstract setVolume(volume: Volume): void;
  abstract setTimer(volume: Timer): void;
  abstract setVoice(volume: Voice): void;
}

export class SettingsMock implements Settings {
  volume = signal<Volume | undefined>(new Volume(50));
  timer = signal<Timer | undefined>(new Timer(10));
  voice = signal<Voice | undefined>(undefined);

  setVolume(volume: Volume): void {
    this.volume.set(volume);
  }

  setTimer(timer: Timer): void {
    this.timer.set(timer);
  }

  setVoice(voice: Voice): void {
    this.voice.set(voice);
  }
}

@Injectable()
export class SettingsService implements Settings, OnDestroy {
  private readonly snackbar = inject(MatSnackBar);
  private readonly http = inject(HttpClient);
  private readonly volumeResource = rxResource({
    stream: () => this.http.get<Volume>('/api/volume'),
  });
  private readonly timerResource = rxResource({
    stream: () => this.http.get<Timer>('/api/timer'),
  });
  private readonly voiceResource = rxResource({
    stream: () => this.http.get<Voice>('/api/voice'),
  });
  private readonly destroy$: Subject<void> = new Subject<void>();

  readonly volume = computed(() => this.volumeResource.value());
  readonly timer = computed(() => this.timerResource.value());
  readonly voice = computed(() => this.voiceResource.value());

  public setTimer(timer: Timer): void {
    this.destroy$.next();
    this.http
      .put('/api/timer', timer, { headers: { 'Content-Type': 'application/json' } })
      .subscribe(() => {
        interval(1000).pipe(takeUntil(this.destroy$)).subscribe(() => this.timerResource.reload());
        this.snackbar.open('Установлен таймер', 'Ответ сервера', { duration: 2000 });
      });
  }

  public setVolume(volume: Volume): void {
    this.http
      .put('/api/volume', volume, { headers: { 'Content-Type': 'application/json' } })
      .subscribe(() => {
        this.volumeResource.reload();
        this.snackbar.open('Установлен звук', 'Ответ сервера', { duration: 2000 });
      });
  }

  public setVoice(voice: Voice): void {
    this.http
      .put('/api/voice', voice, { headers: { 'Content-Type': 'application/json' } })
      .subscribe(() => {
        this.voiceResource.reload();
        this.snackbar.open('Установлено прослушивание', 'Ответ сервера', { duration: 2000 });
      });
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
