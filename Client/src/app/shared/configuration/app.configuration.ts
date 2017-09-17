import { Injectable } from '@angular/core';
import { ToasterConfig } from 'angular2-toaster/angular2-toaster';

import { environment } from '../../../environments/environment';

@Injectable()
export class Configuration {
    server = 'http://localhost:5000/'//'http://52.138.147.79/'; // 'http://localhost:5000/';
    apiUrl = 'api/v1/';
    title = 'eMeal';

    authConfig = {
        CLIENT_ID: 'AngularFoodClient',
        GRANT_TYPE: 'password',
        SCOPE: 'WebAPI'
    }

    toasterConfig: ToasterConfig = new ToasterConfig({
        positionClass: 'toast-bottom-right'
    });
}
