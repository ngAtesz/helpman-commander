import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-competition-editor',
  templateUrl: './competition-editor.component.html',
  styleUrls: ['./competition-editor.component.css']
})
export class CompetitionEditorComponent implements OnInit {
  public competitions: any[];
  public createStarted = false;

  newCompetitionForm = this.fb.group({
    city: [null, Validators.required],
    date: [null, Validators.required],
    category: [null, Validators.required],
  });

  categories = [
    { name: 'Gyermek', categoryId: 1},
    { name: 'Ifjúsági', categoryId: 2},
    { name: 'Felnőtt', categoryId: 3},
  ];

  constructor(http: HttpClient, @Inject('API_BASE_URL') baseUrl: string, private fb: FormBuilder) {
    http.get<any[]>(baseUrl + 'competitions')
      .subscribe(result => {
        this.competitions = result;
      },
        error => console.error(error));
  }

  ngOnInit() {
  }

  onSubmit() {
  }

  startCreate() {
    this.createStarted = true;
  }

  cancel() {
    this.createStarted=false;
    this.newCompetitionForm.reset();
    this.newCompetitionForm.clearValidators();
  }
}
