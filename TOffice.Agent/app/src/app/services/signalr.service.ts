import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;

  constructor() { }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl('http://localhost:5000/tofficehub').build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection established.');
        console.log('Retrieving connection ID...');

        this.hubConnection.invoke('GetConnectionId')
          .then(connectionId => {
            // TODO: put connection ID in session storage
            console.log('Connection ID: ' + connectionId);

          });
      })
      .catch(err => {
        console.error('Error while establishing SignalR connection: ' + err);
      })
  };

  public stopConnection = () => {
    this.hubConnection.stop();
  };
}
