import { NgModule, PLATFORM_ID  } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppModuleShared } from './app.shared.module';
import { AppComponent } from './components/app/app.component';
import { isPlatformBrowser } from '@angular/common';
import { StorageService, BrowserStorage, ServerStorage } from './services/storage.service';

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        AppModuleShared
    ],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl },
        {
            provide: StorageService,
            useClass: isPlatformBrowser ? BrowserStorage : ServerStorage
        }
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
