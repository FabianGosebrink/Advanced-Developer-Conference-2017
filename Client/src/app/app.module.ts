import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PreloadAllModules, RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { ToasterModule } from 'angular2-toaster/angular2-toaster';
import { NgxElectronModule } from 'ngx-electron';

import { AppComponent } from './app.component';
import { AppRoutes } from './app.routes';
import { CoreModule } from './core/core.module';
import { HomeModule } from './home/home.module';
import { SharedModule } from './shared/shared.module';

@NgModule({
    imports: [
        BrowserAnimationsModule,
        BrowserModule,
        ToasterModule,
        RouterModule.forRoot(AppRoutes, { useHash: true, preloadingStrategy: PreloadAllModules }),
        SharedModule,
        NgxElectronModule,
        HomeModule,
        CoreModule.forRoot(),
        StoreModule.forRoot({}),
        StoreDevtoolsModule.instrument({
            maxAge: 25 //  Retains last 25 states
        }),
        EffectsModule.forRoot([])
    ],

    declarations: [
        AppComponent
    ],

    bootstrap: [AppComponent]
})

export class AppModule { }
