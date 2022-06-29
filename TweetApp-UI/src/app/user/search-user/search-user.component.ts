import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { User } from '../user.model';
import { UserService } from '../user.service';
import { ViewUser } from '../view-user.model';

@Component({
  selector: 'app-user',
  templateUrl: './search-user.component.html',
  styleUrls: ['./search-user.component.css']
})
export class SearchUserComponent implements OnInit, OnDestroy {
  users: ViewUser[];
  allUserSubscription : Subscription;  
  userSubscription : Subscription;
  userDetail : ViewUser;
  constructor(private route: ActivatedRoute,private userService: UserService , private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe((param) => {
      this.userService.getUsersById(param['id']);
    });

    this.allUserSubscription = this.userService.allUsers.subscribe((users) => {
      this.users = users;
      console.log(users);
    });
   
  }

  ngOnDestroy(){
    this.allUserSubscription.unsubscribe();
  }
  OnNavigate(){
    this.router.navigate(['tweetapp/home/users']);
  }
}
