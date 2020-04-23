import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';

@Component({
  selector: 'app-value',
  templateUrl: './Value.component.html',
  styleUrls: ['./Value.component.css']
})
export class ValueComponent implements OnInit {
   values: any;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getvalues();
  }
getvalues(){
this.http.get('http://localhost:5000/api/values').subscribe(response => {
this.values = response;

}, Error => {
console.error(Error);
} );

}

}
