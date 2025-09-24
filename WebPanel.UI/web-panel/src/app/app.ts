import { Component, model, signal } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Settings, SettingsMock, SettingsService } from './services/settings';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [RouterOutlet, RouterLink, FormsModule, MatSnackBarModule],
  providers: [{ provide: Settings, useClass: SettingsService }],
  styleUrl: './app.css',
})
export class App {
  menuOpen = model(false);
}
