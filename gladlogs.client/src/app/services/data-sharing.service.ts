import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataSharingService {
  private chatnameSubject = new BehaviorSubject<string>('');
  chatname$: Observable<string> = this.chatnameSubject.asObservable();

  private usernameSubject = new BehaviorSubject<string>('');
  username$: Observable<string> = this.usernameSubject.asObservable();

  updateValues(chatname: string, username: string) {
    this.chatnameSubject.next(chatname);
    this.usernameSubject.next(username);
  }
}