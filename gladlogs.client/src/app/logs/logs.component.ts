import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrl: './logs.component.css'
})
export class LogsComponent implements OnInit {

  username :string = "";
  chatname :string = "";

  constructor(private http: HttpClient, private route: ActivatedRoute) {}
  ngOnInit(): void {

    this.route.params.subscribe({
      next: value => 
      {
        this.chatname = value['chat'];
        this.username = value['nickname'];
      },
      error: err => console.error('Error Loading Messages: ' + err),

    });



  }
}
