import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth-guard';
import { ErrorComponent } from './error/error.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { RegisterComponent } from './register/register.component';
import { ResetPasswordComponent } from './register/reset-password/reset-password.component';
import { EditTweetComponent } from './tweet/edit-tweet/edit-tweet.component';
import { MyTweetsComponent } from './tweet/my-tweets/my-tweets.component';
import { ReplyTweetComponent } from './tweet/reply-tweet/reply-tweet.component';
import { TweetComponent } from './tweet/tweet.component';
import { UserComponent } from './user/user.component';
import { SearchUserComponent } from './user/search-user/search-user.component';
import { UpdateProfileComponent } from './profile/updateProfile/update-profile.component';
import { EmailVerificationComponent } from './email-verification/email-verification.component';
import { NotifyUserComponent } from './user/notify-user/notify-user.component';

const routes: Routes = [
  {path: '', redirectTo: 'tweetapp', pathMatch: "full"},
  {path: 'tweetapp/email-verification', component:EmailVerificationComponent},
  {path: 'tweetapp/home', component: HomeComponent, children: [
    {path: 'tweets/reply/:id', component: ReplyTweetComponent},
    {path: 'tweets/edit/:id', component: EditTweetComponent},
    {path: 'users', component: UserComponent},
    {path: 'mytweets/user/:id', component: MyTweetsComponent},
    {path: 'my-profile/:id', component: ProfileComponent},
    {path: 'tweets', component: TweetComponent},
    {path: 'my-profile/update-profile/:id',component: UpdateProfileComponent},
    {path: 'users/notify-user/:id',component: NotifyUserComponent},
    {path:'users/search-user/:id' , component:SearchUserComponent},
    {path: '', redirectTo: 'tweets',pathMatch: 'full'}
  ], canActivate:[AuthGuard]},
  {path: 'tweetapp', component: RegisterComponent},
  {path: 'tweetapp/reset-password', component: ResetPasswordComponent},     
  {path: '**', component: ErrorComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
