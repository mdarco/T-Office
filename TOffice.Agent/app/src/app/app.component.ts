import { Component } from '@angular/core';
import { ElectronService } from 'ngx-electron';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  tileConfig = {
    titleBackgroundColor: '#ebf5fb',
    btnGroupBackgroundColor: '#fcf3cf',
    colorStatusConnected: '#48c9b0',
    colorStatusNotConnected: '#e74c3c'
  };

  constructor(private electronService: ElectronService) { }

  openApp() {
    // TODO: add wsConnectionId as query string param
    this.electronService.shell.openExternal('http://www.google.com');
  }
}
