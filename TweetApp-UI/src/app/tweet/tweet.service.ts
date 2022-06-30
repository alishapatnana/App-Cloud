import { HttpClient } from "@angular/common/http";
import { Variable } from "@angular/compiler/src/render3/r3_ast";
import { Injectable } from "@angular/core";
import { waitForAsync } from "@angular/core/testing";
import { Subject } from "rxjs";
import { Reply } from "./reply.model";
import { Tweet } from "./tweet.model";
import { environment } from "src/environments/environment.prod";

@Injectable({
    providedIn: 'root'
})
export class TweetService{

    constructor(private http: HttpClient){}
    tweetChanged = new Subject<Tweet>();
    allTweets = new Subject<Tweet[]>();
    private tweets: Tweet[];
    count:string;
    //uri:string = "https://localhost:44396/";
    //uri:string = "https://comtweetapp.azurewebsites.net/";
    uri:string = environment.uri;
    getAllTweets(){
        let tweets: Tweet[];
        this.http.get<Tweet[]>(this.uri+'api/v1.0/tweets/all').subscribe((responseData) => {
            tweets = responseData;
            this.allTweets.next(tweets);

        });
    }

    getTweetById(id: string){
        let tweet: Tweet;
        this.http.get<Tweet>(this.uri+'api/v1.0/tweets/tweet/'+id).subscribe((responseData) => {
            this.tweetChanged.next(responseData);
        });
    }

    addReply(reply: Reply, tweetId: string){
        // this.tweets.find(x => x.id == tweetId)?.replies.push(reply);
        // this.tweetChanged.next(this.tweets.slice().find(x => x.id == tweetId));
        let userId = localStorage.getItem('user');
        this.http.put(this.uri+'api/v1.0/tweets/'+ userId + '/reply/'+tweetId, { tweetText : reply.replyText})
        .subscribe((response) => {
            this.getTweetById(tweetId);
        });
    }

    postTweet(userId: string | null, tweetText: string){
       this.http.post(this.uri+'api/v1.0/tweets/'+ userId +'/add', {tweetText: tweetText}).subscribe((response) => {
           this.getAllTweets();
       });
    }

    editTweet(tweetText: string, tweetId: string){
        this.http.put(this.uri+'api/v1.0/tweets/update/'+ tweetId, { tweetText : tweetText})
        .subscribe((response) => {
            this.getAllTweets();
        });
    }

    getTweetByUserId(userId: string){
        let tweets: Tweet[];
        this.http.get<Tweet[]>(this.uri+'api/v1.0/tweets/'+userId).subscribe((responseData) => {
            tweets = responseData;
            this.allTweets.next(tweets);
        });
    }

    deleteTweetFromHome(tweetId: string){
        this.http.delete(this.uri+'api/v1.0/tweets/delete/' + tweetId).subscribe((response) => {
            this.getAllTweets();
        });
    }

    deleteTweetFromMyTweet(tweetId: string, userId: string){
        this.http.delete(this.uri+'api/v1.0/tweets/delete/' + tweetId).subscribe((response) => {
            this.getTweetByUserId(userId);
        });
    }

    likeOrDisLikeTweet(tweetId: string, userId: string){
        this.http.put(this.uri+'api/v1.0/tweets/'+userId+'/like/'+tweetId, null).subscribe((response) => {
            this.getTweetByUserId(userId);
        });
    }

    inActivateReply( userId: string){
        this.http.put(this.uri+'api/v1.0/tweets/inactivateReply/'+ userId , null).subscribe((response) => {
            this.getAllTweets();
        });
    }

    getActiveReplies( userId: string){  
        return this.http.get<any>(this.uri+'api/v1.0/tweets/activeReplies/'+ userId).subscribe((data)=>{
           localStorage.setItem('count',data);
       });  
    }
}