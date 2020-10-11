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
    colorStatusReconnecting: 'blue',
    colorStatusNotConnected: '#e74c3c'
  };

  status = {
    connected: false,
    reconnecting: false,
    notConnected: true
  };

  hubConnection = null;

  constructor(
    private signalrService: SignalrService,
    private electronService: ElectronService
  ) { }

  ngOnInit() {
    this.startSignalrConnection();
    this.signalrService.addReadSmartCardDataListener();
  }

  ngOnDestroy() {
    this.signalrService.stopConnection();
  }

  startSignalrConnection() {
    this.signalrService.startConnection()
      .then(() => {
        console.log('SignalR connection established.');
        console.log('Retrieving connection ID...');

        this.status = {
          connected: true,
          reconnecting: false,
          notConnected: false
        };

        this.hubConnection = this.signalrService.getConnection();

        this.hubConnection.invoke('GetConnectionId')
          .then(connectionId => {
            sessionStorage.setItem('wsConnectionId', connectionId);
            console.log('Connection ID: ' + connectionId);
          });

        this.handleReconnectEvents();
      })
      .catch(err => {
        console.error('Error while establishing SignalR connection: ' + err);
      });
  }

  handleReconnectEvents() {
    this.hubConnection.onreconnecting(err => {
      console.error('SignalR reconnection reason: ' + err);

      this.status = {
        connected: false,
        reconnecting: true,
        notConnected: false
      };
    });

    this.hubConnection.onreconnected(connectionId => {
      this.status = {
        connected: true,
        reconnecting: false,
        notConnected: false
      };

      sessionStorage.setItem('wsConnectionId', connectionId);
    });

    this.hubConnection.onclose(err => {
      console.error('SignalR connection closed reason: ' + err);

      this.status = {
        connected: false,
        reconnecting: false,
        notConnected: true
      };
    });
  }

  manualSignalrConnect() {
    this.startSignalrConnection();
  }

  openApp() {
    const wsConnectionId = sessionStorage.getItem('wsConnectionId');
    this.electronService.shell.openExternal('http://localhost:52013/index.html?cid=' + wsConnectionId);
  }
}
