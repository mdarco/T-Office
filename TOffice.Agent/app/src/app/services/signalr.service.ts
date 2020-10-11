import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;

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

  public stopConnection = () => {
    this.hubConnection.stop();
  };
}
