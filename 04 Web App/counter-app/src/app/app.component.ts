import { Component } from "@angular/core";
import * as signalR from "@aspnet/signalr";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { map, switchMap } from "rxjs/operators";

interface SignalRConnection {
  url: string;
  accessToken: string;
}

interface Counter {
  id: number;
  count: number;
}

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"]
})
export class AppComponent {
  private readonly httpOptions = { headers: new HttpHeaders({ "Content-Type": "application/json" }) };
  private readonly negotiateUrl = "http://localhost:7071/api/negotiate";
  private readonly getCounterUrl = "http://localhost:7071/api/get-counter";
  private readonly updateCounterUrl = "http://localhost:7071/api/update-counter";

  private readonly counterId = 1;

  private hubConnection: signalR.HubConnection;
  private counter: number = 0;

  constructor(private readonly http: HttpClient) {
    const negotiateBody = { UserId: "SomeUser" };

    this.http
      .post<SignalRConnection>(this.negotiateUrl, JSON.stringify(negotiateBody), this.httpOptions)
      .pipe(
        map(connectionDetails =>
          new signalR.HubConnectionBuilder().withUrl(`${connectionDetails.url}`, { accessTokenFactory: () => connectionDetails.accessToken }).build()
        )
      )
      .subscribe(hub => {
        this.hubConnection = hub;
        hub.on("CounterUpdate", data => {
          console.log(data);
          this.counter = data.Count;
        });
        hub.start();
      });

    this.http.get<Counter>(this.getCounterUrl + "/" + this.counterId).subscribe(cloudCounter => {
      console.log(cloudCounter);
      this.counter = cloudCounter.count;
    });
  }

  public increaseCounter(): void {
    console.log("HERE!");
    const body = { Id: this.counterId, Count: this.counter };

    this.http
      .post(this.updateCounterUrl, body, this.httpOptions)
      .toPromise()
      .catch(e => console.log(e));
  }
}
