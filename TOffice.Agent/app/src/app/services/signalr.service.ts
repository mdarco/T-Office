import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { ChildProcessService } from 'ngx-childprocess';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;

  constructor(
    private childProcessService: ChildProcessService
  ) { }

  public getConnection = () => {
    return this.hubConnection;
  };

  public startConnection = () => {
    this.hubConnection =  new signalR.HubConnectionBuilder()
                            .withUrl('http://localhost:5000/tofficehub')
                            .withAutomaticReconnect()
                            .configureLogging(signalR.LogLevel.Information)
                            .build();

    return this.hubConnection.start();
  };

  public addReadSmartCardDataListener = () => {
    this.hubConnection.on('readSmartCardData', () => {
      // call RegLicenseReader (from Electron)
      const result = this.childProcessService.childProcess.execFileSync('c:\\Temp\\RegLicenseReader.exe', null, {});
      const stringResult = new TextDecoder('utf-8').decode(result);

      console.info('SMART CARD DATA!');
      console.log(stringResult);
    });
  };

  public stopConnection = () => {
    this.hubConnection.stop();
  };
}
