import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cookies-component',
  templateUrl: './cookies-component.component.html',
  styleUrls: ['./cookies-component.component.css'],
})
export class CookiesComponentComponent implements OnInit {
  constructor() {
    const today = new Date();
    const yesterday = new Date(today);
    yesterday.setDate(today.getDate() - 1);
    const day = yesterday.getDate();
    const month = yesterday.getMonth() + 1;
    const year = yesterday.getFullYear();
    this.lastUpdated = `${day}/${month}/${year}`;
  }

  lastUpdated: string;

  ngOnInit(): void {}
}
