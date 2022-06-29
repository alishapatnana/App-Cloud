import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Tweet } from 'src/app/tweet/tweet.model';
import { TweetService } from 'src/app/tweet/tweet.service';
import { User } from '../user.model';
import { UserService } from '../user.service';
import { ViewUser } from '../view-user.model';

@Component({
  selector: 'app-user',
  templateUrl: './notify-user.component.html',
  styleUrls: ['./notify-user.component.css']
})
export class NotifyUserComponent implements OnInit, OnDestroy {
  users: ViewUser[];
  tweets: Tweet[]; 
  userId:string;
  tweetSubscription : Subscription;
  userDetail : ViewUser;
  constructor(private route: ActivatedRoute,private userService: UserService , private tweetService: TweetService, private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe((param) => {
      this.tweetService.getTweetByUserId(param['id']),
      this.userId=param['id'];
    });

    this.tweetSubscription =  this.tweetService.allTweets.subscribe((tweets) => {
      console.log(tweets);
      this.tweets = tweets;
    });
  }

  ngOnDestroy(){
    this.tweetService.inActivateReply(this.userId);
    window.location.reload();
  }
}