import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { ChildProcessService } from 'ngx-childprocess';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;

  private SIGNALR_URL = environment.signalrUrl;
  private API_URL = environment.apiUrl;
  private REG_LICENSE_READER_PATH = environment.regLicenseReaderPath;

  private readSmartCardData$: Subscription;

  constructor(
    private childProcessService: ChildProcessService,
    private http: HttpClient
  ) { }

  ngOnDestroy() {
    if (this.readSmartCardData$) {
      this.readSmartCardData$.unsubscribe();
    }
  }

  public getConnection = () => {
    return this.hubConnection;
  };

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl(this.SIGNALR_URL)
                            .withAutomaticReconnect()
                            .configureLogging(signalR.LogLevel.Information)
                            .build();

    return this.hubConnection.start();
  };

  public addReadSmartCardDataListener = () => {
    this.hubConnection.on('readSmartCardData', () => {
      // call RegLicenseReader.exe (from Electron)
      const result = this.childProcessService.childProcess.execFileSync(this.REG_LICENSE_READER_PATH, null, {});
      const stringResult = new TextDecoder('utf-8').decode(result);

      console.info('SMART CARD DATA:');
      console.log(stringResult);

      this.hubConnection.invoke('GetConnectionId')
        .then(connectionId => {
          // send smart card data to api internal persistent store
          // data will be picked up by the web app (that issued the request)
          // const url = 'http://localhost:5000/api/reg-license-reader/response/' + connectionId;
          const url = `${this.API_URL}/reg-license-reader/response/${connectionId}`;

          const model = {
            WsConnectionId: connectionId,
            Data: stringResult
          };

          this.readSmartCardData$ = this.http.post(url, model).subscribe();
        });
    });
  };

  public insertAgentData = (agentData) => {
    console.log('SignalR service calling API InsertAgentData with agent data: ' + JSON.stringify(agentData));
    this.hubConnection.invoke('InsertAgentData', agentData)
      .catch(err => {
        console.error('SignalR service calling API InsertAgentData error: ');
        console.error(err);
      });
  }

  public stopConnection = () => {
    this.hubConnection.stop();
  };
}
