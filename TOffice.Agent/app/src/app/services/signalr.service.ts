import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;

  constructor() { }

  public startConnection = () => {
    this.hubConnection =  new signalR.HubConnectionBuilder()
                            .withUrl('http://localhost:5000/tofficehub')
                            .withAutomaticReconnect()
                            .configureLogging(signalR.LogLevel.Information)
                            .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection established.');
        console.log('Retrieving connection ID...');

        this.hubConnection.invoke('GetConnectionId')
          .then(connectionId => {
            sessionStorage.setItem('wsConnectionId', connectionId);
            console.log('Connection ID: ' + connectionId);

            this.hubConnection.invoke('JoinGroupWithConnectionId', connectionId)
              .then(() => {
                console.log('Joined to SignalR group.');
              });
          });
      })
      .catch(err => {
        console.error('Error while establishing SignalR connection: ' + err);
      });

    this.hubConnection.onreconnecting(error => {
      // TODO: change status to 'RECONNECTING'
    });

    this.hubConnection.onreconnected(connectionId => {
      // TODO: change status to 'CONNECTED' and update 'wsConnectionId' in session storage and rejoin the group on the sever
      sessionStorage.setItem('wsConnectionId', connectionId);
    });

    this.hubConnection.onclose(error => {
      // TODO: change status to 'NOT CONNECTED'
    });
  };

  public stopConnection = () => {
    this.hubConnection.stop();
  };
}
