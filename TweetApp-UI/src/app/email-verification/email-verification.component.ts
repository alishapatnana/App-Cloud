import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-email-verification',
  templateUrl: './email-verification.component.html',
  styleUrls: ['./email-verification.component.css']
})
export class EmailVerificationComponent implements OnInit {
  userOtp:string;
  userEnterOtp:string;
  userMail:string;
  constructor(private router: Router,private authService: AuthService) { }

  ngOnInit(): void {
  }
  onValidate(form : NgForm){
  this.userEnterOtp = form.value.otp;
  this.userOtp = localStorage.getItem('userOtp') as string;
  if(this.userEnterOtp === this.userOtp){
    this.router.navigate(['tweetapp/home']);
  }
  else{
    this.router.navigate(['tweetapp/email-verification']);
    form.reset();
  }
  }
  sendMail(){
    this.userMail = localStorage.getItem('user') as string;
    this.authService.mailSend(this.userMail);
    this.router.navigate(['tweetapp/email-verification']);
  }
}
