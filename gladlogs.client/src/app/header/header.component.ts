import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataSharingService } from '../services/data-sharing.service';



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
export class HeaderComponent implements OnInit  {
  username: string = '';
  chatname: string = '';

  public chats: GetAllChatsResponse = {chatnames:[]};

  ngOnInit() {
    this.dataService.chatname$.subscribe(value => this.chatname = value);
    this.dataService.username$.subscribe(value => this.username = value);

    this.getChatNames();

    onsubmit = this.onsubmit;


  }
  constructor(private http: HttpClient, private route: ActivatedRoute, private dataService: DataSharingService) {
    
  }
  getChatNames(){

    this.http.get<GetAllChatsResponse>('/api/chats').subscribe(
      {
        next: (value) => {
          this.chats = value;
        },
        error: (error) => {
          console.error(error);
        }
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

  onsubmit(e : SubmitEvent){
    e.preventDefault();
    console.log("ez")
    this.onSearch();
  }


}
