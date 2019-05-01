import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-competition-editor',
  templateUrl: './competition-editor.component.html',
  styleUrls: ['./competition-editor.component.css']
})
export class CompetitionEditorComponent implements OnInit {
  public competitions: any[];

  constructor(http: HttpClient, @Inject('API_BASE_URL') baseUrl: string) {
    http.get<any[]>(baseUrl + 'competitions')
      .subscribe(result => {
        this.competitions = result;
      },
        error => console.error(error));
  }

  ngOnInit() {
  }

}