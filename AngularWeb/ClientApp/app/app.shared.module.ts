import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, BrowserXhr } from '@angular/http';
import { RouterModule } from '@angular/router';
import { NgProgressModule, NgProgressBrowserXhr } from 'ngx-progressbar';
import { isPlatformBrowser } from '@angular/common';
import { StorageService, BrowserStorage, ServerStorage } from './services/storage.service';
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { LeaderboardComponent } from './components/leaderboard/leaderboard.component';
import { CounterComponent } from './components/counter/counter.component';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { UserPicksComponent } from './components/userPicks/userPicks.component';
import { GameSelectorComponent } from './components/gameselector/gameselector.component';
import { GameResultsComponent } from './components/gameresults/gameresults.component';
import { WeeklyResultsComponent } from './components/weeklyResults/weeklyResults.component';
import { RulesComponent } from './components/rules/rules.component';
import { ForgotPasswordComponent } from './components/forgotPassword/forgotPassword.component';
import { ResetPasswordComponent } from './components/resetPassword/resetPassword.component';
import { ProfileComponent } from './components/profile/profile.component';
import { MessageComponent } from './components/message/message.component';
import { AuthService } from './services/auth.service';
import { CriteriaService } from './services/criteria.service';




@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        LoginComponent,
        LeaderboardComponent,
        RegistrationComponent,
        UserPicksComponent,
        GameSelectorComponent,
        GameResultsComponent,
        WeeklyResultsComponent,
        ForgotPasswordComponent,
        RulesComponent,
        ResetPasswordComponent,
        ProfileComponent,
        MessageComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        NgProgressModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'login', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: 'login', component: LoginComponent },
            { path: 'leaderboard', component: LeaderboardComponent },
            { path: 'registration', component: RegistrationComponent },
            { path: 'userPicks', component: UserPicksComponent },
            { path: 'gameselector', component: GameSelectorComponent },
            { path: 'gameresults', component: GameResultsComponent },
            { path: 'weeklyResults', component: WeeklyResultsComponent },
            { path: 'rules', component: RulesComponent },
            { path: 'forgotPassword', component: ForgotPasswordComponent },
            { path: 'resetPassword', component: ResetPasswordComponent },
            { path: 'profile', component: ProfileComponent },
            { path: 'message', component: MessageComponent},
            { path: '**', redirectTo: 'login' }
        ])
    ],
    providers: [
        AuthService,
        CriteriaService,
        { provide: BrowserXhr, useClass: NgProgressBrowserXhr },
        {
            provide: StorageService,
            useClass: isPlatformBrowser ? BrowserStorage : ServerStorage
        }
    ],
    exports: [
        RouterModule
    ],
})
export class AppModuleShared {
}
