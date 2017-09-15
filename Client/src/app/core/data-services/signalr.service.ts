import { Configuration } from '../../shared/configuration/app.configuration';

import { EventEmitter, Injectable } from '@angular/core';
// import * as signalR from '@aspnet/signalr-client';

declare var signalR: any;

@Injectable()
export class SignalRService {

    foodAdded = new EventEmitter<any>();

    private _hubConnection: any;

    constructor(private configuration: Configuration) {
        this._hubConnection = new signalR.HubConnection(configuration.server + 'foodhub');

        this.registerOnServerEvents();

        this.startConnection();
    }

    private startConnection(): void {

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
            })
            .catch(err => {
                console.log('Error while establishing connection')
            });
    }

    private registerOnServerEvents(): void {

        this._hubConnection.on('foodadded', (data: any) => {
            console.log('foodadded');
            this.foodAdded.emit(data);
        });

    }
}
