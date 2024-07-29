import { Byte } from '@angular/compiler/src/util';

export default class Story {
  Id: any;
  Publisher: any;
  Title: string;
  Description: string;
  AuthorName: string;
  Genre: any;
  AgeLimit: number;
  ImagePreview: Array<Byte> | undefined;
  DatePublished: Date | undefined;
  DateWritten: Date | undefined;

  constructor(id: any, title: string, desc: string, aname: string,
    alimit: number, preview: Array<Byte>, datep: Date, datew: Date) {
    this.Id = id;
    this.Title = title;
    this.Description = desc;
    this.AuthorName = aname;
    this.AgeLimit = alimit;
    this.ImagePreview = preview;
    this.DatePublished = datep;
    this.DateWritten = datew;
  }
}
