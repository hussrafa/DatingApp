import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/Auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {
  FModel: any = {};
  // tslint:disable-next-line: no-shadowed-variable
  constructor(private AuthService: AuthService) { }

  ngOnInit() {
  }

  FormSubmit(){
    this.AuthService.login(this.FModel).subscribe(next => {
      console.log('login successful');
    }, error => {
      console.log(error);
    });
  }

  loggedin(){
    const token = localStorage.getItem('token');
    return !! token;

  }

  logout(){
      localStorage.removeItem('token');
      console.log('logged out');
  }

}
