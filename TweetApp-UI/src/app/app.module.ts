import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderComponent } from './header/header.component';
import { TweetComponent } from './tweet/tweet.component';
import { UserComponent } from './user/user.component';
import { SearchUserComponent } from './user/search-user/search-user.component';
import { TweetItemsComponent } from './tweet/tweet-items/tweet-items.component';
import { PostTweetComponent } from './tweet/post-tweet/post-tweet.component';
import { TweetService } from './tweet/tweet.service';
import { AuthService } from './auth.service';
import { UserService } from './user/user.service';
import { ReplyTweetComponent } from './tweet/reply-tweet/reply-tweet.component';
import { EditTweetComponent } from './tweet/edit-tweet/edit-tweet.component';
import { FormsModule ,  ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import { AuthGuard } from './auth-guard';
import { MyTweetsComponent } from './tweet/my-tweets/my-tweets.component';
import { ProfileComponent } from './profile/profile.component';
import { UpdateProfileComponent } from './profile/updateProfile/update-profile.component';
import { ResetPasswordComponent } from './register/reset-password/reset-password.component';
import { ErrorComponent } from './error/error.component';
import { EmailVerificationComponent } from './email-verification/email-verification.component';
import { NotifyUserComponent } from './user/notify-user/notify-user.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    RegisterComponent,
    HeaderComponent,
    TweetComponent,
    UserComponent,
    TweetItemsComponent,
    PostTweetComponent,
    ReplyTweetComponent,
    EditTweetComponent,
    MyTweetsComponent,
    ProfileComponent,
    ResetPasswordComponent,
    ErrorComponent,
    UpdateProfileComponent,
    SearchUserComponent,
    EmailVerificationComponent,
    NotifyUserComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [HttpClientModule, AuthService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
