import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { ChildProcessService } from 'ngx-childprocess';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;

  constructor(
    private childProcessService: ChildProcessService,
    private http: HttpClient
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
      // call RegLicenseReader.exe (from Electron)
      const result = this.childProcessService.childProcess.execFileSync('c:\\Temp\\RegLicenseReader.exe', null, {});
      const stringResult = new TextDecoder('utf-8').decode(result);

      console.info('SMART CARD DATA:');
      console.log(stringResult);

      this.hubConnection.invoke('GetConnectionId')
        .then(connectionId => {
          // send smart card data to api internal persistent store
          // data will be picked up by the web app (who issued the request)
          const url = 'http://localhost:5000/api/reg-license-reader/response/' + connectionId;

          const model = {
            WsConnectionId: connectionId,
            Data: stringResult
          };

          this.http.post(url, model).subscribe();
        });
    });
  };

  public stopConnection = () => {
    this.hubConnection.stop();
  };
}
