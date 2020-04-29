import { Component, OnInit , EventEmitter } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  register = false;
  constructor() { }

  ngOnInit() {
  }

  registerclick(){
      this.register =  true;

  }
  OnCancel(register: boolean)
  {
    console.log(register);
    this.register = register;
  }

}
