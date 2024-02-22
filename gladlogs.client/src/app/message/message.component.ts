import { Component, Input, OnInit } from '@angular/core';
import { GetAllMessages, Message } from '../../types/GetMessages';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent {


GetDate(arg0: Date) {
  arg0 = new Date(arg0);
  return `${arg0.toLocaleString("pl-PL")}`;
}

  @Input() 
  username!: string;

  @Input() 
  messages!: GetAllMessages;



}
