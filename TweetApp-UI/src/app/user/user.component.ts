import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { User } from './user.model';
import { UserService } from './user.service';
import { ViewUser } from './view-user.model';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit, OnDestroy {
  users: ViewUser[];
  allUserSubscription : Subscription;
  constructor(private userService: UserService, private router: Router) { }
  search : string;
  ngOnInit(): void {
    this.userService.getAllUsers();
    this.allUserSubscription = this.userService.allUsers.subscribe((users) => {
      this.users = users;
      console.log(users);
    });
  }

  ngOnDestroy(){
    this.allUserSubscription.unsubscribe();
  }

  onSubmit(form : NgForm){
    this.search = form.value.search;
    if(this.search == undefined){
      this.router.navigate(['tweetapp/home/users']).then(() => {
        window.location.reload();
      });
    }
    else{
      this.router.navigate(['tweetapp/home/users/search-user/'+this.search]).then(() => {
        window.location.reload();
      });
    }
  }
}
