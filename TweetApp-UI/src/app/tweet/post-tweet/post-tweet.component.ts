import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Tweet } from '../tweet.model';
import { TweetService } from '../tweet.service';

@Component({
  selector: 'app-post-tweet',
  templateUrl: './post-tweet.component.html',
  styleUrls: ['./post-tweet.component.css']
})
export class PostTweetComponent implements OnInit {
  @ViewChild('f') form: NgForm;
  userId : string | null;
  userGender:string = localStorage.getItem('gender') as string;
  constructor(private tweetService: TweetService) { }

  ngOnInit(): void {
    this.userId = localStorage.getItem('user');
  }

  onSubmit(){
   
    if(this.userId!=null){
      this.tweetService.postTweet(this.userId, this.form.value.tweetText);
    }
    else{
      return;
    }
    this.form.reset();
  }
}
