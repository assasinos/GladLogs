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
  styleUrl: './header.component.css',
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


  onSearch() {
    //Check if username supplied
    //If not, return
    const username = (document.getElementById('username') as HTMLInputElement).value;
    if(username === '' ){
      return;
    }

    //Get Chatname
    const chatname = (document.getElementById('chat') as HTMLInputElement).value;
    if(chatname === ''){
      return;
    }


    //Goto the userlogs page
    window.location.href = `/logs/${chatname}/${username}`;

  }


}
