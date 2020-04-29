import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/Auth.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelevent = new EventEmitter<boolean>();
  DataModel: any = {};

  // tslint:disable-next-line: no-shadowed-variable
  constructor(private AuthService: AuthService) { }

  ngOnInit() {
  }


  Onsubmit(){
    this.AuthService.register(this.DataModel).subscribe(() => {
     console.log('registeration successful') ;
    }, error => {
     console.log(error);
    });
  }

  Cancelclick(){
    this.cancelevent.emit(false);
    console.log('event clicked');
  }
}
