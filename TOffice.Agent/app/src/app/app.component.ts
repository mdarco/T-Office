import { Component, OnInit, OnDestroy } from '@angular/core';
import { ElectronService } from 'ngx-electron';
import { SignalrService } from './services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  tileConfig = {
    titleBackgroundColor: '#ebf5fb',
    btnGroupBackgroundColor: '#fcf3cf',
    colorStatusConnected: '#48c9b0',
    colorStatusNotConnected: '#e74c3c'
  };

  constructor(
    private signalrService: SignalrService,
    private electronService: ElectronService
  ) { }

  ngOnInit() {
    this.signalrService.startConnection();
  }

  ngOnDestroy() {
    this.signalrService.stopConnection();
  }

  openApp() {
    // TODO: add wsConnectionId as query string param
    this.electronService.shell.openExternal('http://www.google.com');
  }
}
