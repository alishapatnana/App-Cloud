import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { User } from 'src/app/user/user.model';
import { UserService } from '../../user/user.service';
import { ViewUser } from '../../user/view-user.model';

@Component({
  selector: 'app-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.css']
})
export class UpdateProfileComponent implements OnInit {
  
  constructor(private route: ActivatedRoute, private userService: UserService , private router: Router) { }
  userDetail: ViewUser;
  userSubscription : Subscription;
  user : User;
  isAccountUpdateSuccessfull = false;
  firstName : string;
  lastName : string;
  dob:Date;
  gender:string;
  

  ngOnInit(): void {
    this.route.params.subscribe((param) => {
      this.userService.getUsers(param['id']);
    });

    this.userSubscription =  this.userService.userDetail.subscribe((userDetail) =>{
      this.userDetail = userDetail,
      this.firstName= userDetail.firstName,
      this.lastName = userDetail.lastName
    })
  }

  

onSubmit(form : NgForm){
  if(!form.valid){
    return;
  }
  const userDetail = new ViewUser(
    this.userDetail.emailId,
    form.value.dob, 
    form.value.gender,
    form.value.firstName,
    form.value.lastName
    );
    this.userService.updateProfile(userDetail,this.userDetail.emailId).subscribe((res) =>{
      if(res){
        this.isAccountUpdateSuccessfull = true;        
        this.router.navigate(['tweetapp/home/my-profile/'+this.userDetail.emailId])
        .then(() => {
          window.location.reload();        
      });
    }});    
}
}
