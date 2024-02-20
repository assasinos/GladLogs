import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


interface GetAllChatsResponse{
  chatnames: ChatResponse[]; 
}
interface ChatResponse{
  chatname:string;
}



@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {

  public chats: GetAllChatsResponse = {chatnames:[]};

  ngOnInit() {
    this.getChatNames();
  }
  constructor(private http: HttpClient) {}
  getChatNames(){

    this.http.get<GetAllChatsResponse>('/api/chats').subscribe(
      (result) => {
        this.chats = result;
      },
      (error) => {
        console.error(error);
      }
    );

  }
}
