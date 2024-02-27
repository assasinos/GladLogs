import { HttpClient } from '@angular/common/http';
import { ApplicationRef, Component, ComponentFactoryResolver, OnInit, createComponent } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GetAllActivityWeekResponses } from '../../types/ActivityWeekResponse';
import { GetAllMessages } from '../../types/GetMessages';
import { MessageComponent } from '../message/message.component';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from '../app.component';
import { DataSharingService } from '../services/data-sharing.service';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrl: './logs.component.css'
})
export class LogsComponent implements OnInit {



  onLoadMessages(arg0: number) {

    this.http
    .get<GetAllMessages>(
      `/api/logs/messages/${this.chatname}/${this.username}/${arg0}`
    )
    .subscribe({
      next: async (result) => {
        const messages = result;

        messages.messages.reverse()

        

        // Locate a DOM node that would be used as a host.
        const hostElement = document.getElementById(`week_${arg0}`);
        if (!hostElement) {
          return;
        }
        hostElement.innerHTML = '';
        // Get an `EnvironmentInjector` instance from the `ApplicationRef`.
        const environmentInjector = this.ref.injector;

        // We can now create a `ComponentRef` instance.
        const componentRef = createComponent(MessageComponent, {hostElement, environmentInjector});

        componentRef.setInput('messages',messages);
        componentRef.setInput('username',this.username);
        this.ref.attachView(componentRef.hostView);
        componentRef.changeDetectorRef.detectChanges();

      },
      error: (error) => {
        console.error(error);
      },
    });



  }

  GetDate(arg0: number) {
    const startDate: Date = new Date(Date.now());
    startDate.setDate(startDate.getDate() - arg0 * 7);

    const endDate: Date = new Date(startDate);
    endDate.setDate(endDate.getDate() - 7);

    return `${startDate.getDate()}/${startDate.getMonth()} - ${endDate.getDate()}/${endDate.getMonth()}`;
  }


  username: string = '';
  chatname: string = '';

  activeWeeks: GetAllActivityWeekResponses = { weeks: [] };

  constructor(private http: HttpClient, private route: ActivatedRoute, private ref : ApplicationRef, private dataService: DataSharingService) {}
  ngOnInit(): void {
    //Get params
    this.route.params.subscribe({
      next: (value) => {
        this.dataService.updateValues(value['chat'], value['nickname']);
        this.chatname = value['chat'];
        this.username = value['nickname'];
      },
      error: (err) => console.error('Error Loading Messages: ' + err),
    });

    //Get user active weeks

    this.http
      .get<GetAllActivityWeekResponses>(
        `/api/logs/user/${this.chatname}/${this.username}`
      )
      .subscribe({
        next: (result) => {
          this.activeWeeks = result;
          this.activeWeeks.weeks.reverse();
        },
        error: (error) => {
          console.error(error);
        },
      });
  }
}
