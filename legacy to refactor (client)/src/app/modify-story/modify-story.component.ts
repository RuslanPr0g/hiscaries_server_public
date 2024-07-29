import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TestsharedService } from 'src/app/testshared.service';
import { Router } from '@angular/router';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import * as _ from "lodash"
import { Genre } from '../publish/Genre';

@Component({
  selector: 'app-modify-story',
  templateUrl: './modify-story.component.html',
  styleUrls: ['./modify-story.component.css']
})
export class ModifyStoryComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private service: TestsharedService,
    private route: ActivatedRoute, private router: Router) {
    this.modifystory = this.formBuilder.group({
      storyId: '',
      title: '',
      description: '',
      authorname: '',
      genreId: '',
      agelimit: '',
      datewritten: new Date()
    })

    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '45rem',
    minHeight: '10rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [
      ['bold']
    ],
    customClasses: [
      {
        name: "quote",
        class: "quote",
      },
      {
        name: 'redText',
        class: 'redText'
      },
      {
        name: "titleText",
        class: "titleText",
        tag: "h1",
      },
    ]
  };

  StoryId: any = 0;
  Story: any = {};
  errorMessage: string = "";
  IsError: boolean = false;
  IsLoading: boolean = true;

  profanityError: boolean = false;

  modifystory: FormGroup;
  htmlContent: string = "";
  pages: Array<string> = [""];
  CurrentPage: number = 0;

  CurrentUser: any;

  selectedValue: number[] = [];

  genres: Genre[] = [];
  GenreList: any = [];

  fileName = '';

  ngOnInit(): void {
    if (this.service.isAuthenticated() == false) {
      this.router.navigateByUrl('login');
    }

    let id = this.route.snapshot.params['id'];
    this.StoryId = id;

    this.setUser();
    this.getStory(id);
    this.getStoryPages(id);
    this.setGenres();
  }

  setGenres(): void {
    this.service.getGenres().subscribe(data => {
      this.GenreList = data.reverse();

      this.GenreList.forEach((genre: any) => {
        this.genres.push({ id: genre.id, name: genre.name });
      });
    })
  }

  setUser(): void {
    this.service.getUserInfo()
      .subscribe(res => {
        this.CurrentUser = res;
      },
        error => {
        });
  }

  getStoryPages(id: any): void {
    this.service.getPagesForStory({ id }).subscribe(data => {
      this.pages = data.map((cpage: any) => {
        return cpage.content;
      });

      let isError = this.pages == null || this.Story == undefined ? true : false;

      if (isError) {
        setTimeout(() => {
          this.IsError = true;
        }, 1000);
      }
      else {
        this.htmlContent = this.pages[this.CurrentPage];
      }

      setTimeout(() => {
        this.IsLoading = false;
      }, 1000);
    }, error => {
      this.IsError = true;
    })
  }

  getStory(id: any): any {
    this.service.getStoryById({ id }).subscribe(data => {
      this.Story = data[0];

      this.selectedValue = this.Story.genres.map((x: any) => {
        return x.id
      });

      this.modifystory = this.formBuilder.group({
        title: this.Story.title,
        description: this.Story.description,
        authorname: this.Story.authorName,
        agelimit: this.Story.ageLimit,
        datewritten: this.Story.dateWritten
      })

      let isError = this.Story == null || this.Story == undefined ? true : false;

      if (isError) {
        setTimeout(() => {
          this.IsError = true;
        }, 1000);
      }

      setTimeout(() => {
        if (this.CurrentUser.id != this.Story.publisherId) {
          this.router.navigateByUrl('');
        }

        this.IsLoading = false;
      }, 1000);
    }, error => {
      this.IsError = true;
    })
  }

  modifyStoryInfo(): void {
    if (this.selectedValue.length === 0)
    {
      alert("Select at leat one genre!");
      return;
    }

    var age = this.modifystory.controls['agelimit'].value;

    if (+age <= 0) {
      this.errorMessage = "Please, enter valid age!";
      return;
    }

    let storyMod = this.modifystory.getRawValue();
    storyMod["storyId"] = this.StoryId;
    storyMod["imagePreview"] = this.cardImageBase64;
    storyMod["genreIds"] = this.selectedValue;
    storyMod["storyAudio"] = this.audioFileBase64;

    this.service.updateStoryInfo(storyMod).subscribe(
      data => {
        this.router.navigateByUrl('story/info/' + this.StoryId);
      },
      error => {
        console.error("Modify Error", error)
        let errorstring = "";

        for (const [key, value] of Object.entries(error.error.errors)) {
          errorstring += `${key}: ${value}`;
        }

        if (errorstring.includes("agelimit")) {
          this.errorMessage = "Please, enter valid age!";
          return;
        }
        else {
          alert(errorstring);
        }

        this.errorMessage = error.error;
      });
  }

  deleteStory() {
    this.service.deleteStory({ storyId: this.StoryId }).subscribe(
      data => {
        this.router.navigateByUrl('');
      },
      error => {
        console.log(error)
        this.IsError = true;
        this.errorMessage = error.error;
        this.router.navigateByUrl('');
      });
  }

  imageError: any;
  audioError: any;
  isImageSaved: boolean = false;
  cardImageBase64: any;
  audioFileBase64: any;
  sureToDelete: boolean = false;

  amISureToDelete(event: any) {
    this.sureToDelete = true;
  }

  deleteAudio(event: any) {
    this.sureToDelete = false;
    this.service.deleteAudio(this.StoryId).subscribe(
      data => {
        alert("Audio removed!");
      },
      error => {
        this.IsError = true;
        alert("There is no audio :)");
        location.reload();
      });
  }

  uploadAudio(event: any) {
    const file: File = event.target.files[0];
    const max_size = 20000000;
  
    if (file.size > max_size) {
      this.audioError = 'Maximum size allowed is ' + max_size / 1000000 + 'Mb';
      return;
    }
  
    const reader = new FileReader();
    reader.readAsArrayBuffer(file);
  
    reader.onload = (e: any) => {
      const arrayBuffer = e.target.result;
      const blob = new Blob([arrayBuffer], { type: file.type });
      const fileReader = new FileReader();
  
      fileReader.onload = (event: any) => {
        const base64String = event.target.result;
        this.audioFileBase64 = base64String;
        this.audioError = null;
      };
  
      fileReader.readAsDataURL(blob);
    };
  }
  

  uploadimage(fileInput: any) {
    this.imageError = null;
    if (fileInput.target.files && fileInput.target.files[0]) {
      // Size Filter Bytes
      const max_size = 2000000;
      const allowed_types = ['image/png', 'image/jpeg'];
      const max_height = 15200;
      const max_width = 25600;

      if (fileInput.target.files[0].size > max_size) {
        this.imageError =
          'Maximum size allowed is ' + max_size / 1000000 + 'Mb';

        return false;
      }

      if (!_.includes(allowed_types, fileInput.target.files[0].type)) {
        this.imageError = 'Only Images are allowed ( JPG | PNG )';
        return false;
      }
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const image = new Image();
        image.src = e.target.result;
        image.onload = rs => {
          // fix it
          const img_height = 400;
          const img_width = 800;

          if (img_height > max_height && img_width > max_width) {
            this.imageError =
              'Maximum dimentions allowed ' +
              max_height +
              '*' +
              max_width +
              'px';
            return false;
          } else {
            const imgBase64Path = e.target.result;
            this.cardImageBase64 = imgBase64Path;



            this.isImageSaved = true;
            return true;
            // this.previewImagePath = imgBase64Path;
          }
        };
      };

      reader.readAsDataURL(fileInput.target.files[0]);
    }

    return true;
  }

  nextPage(): void {
    if (this.htmlContent != "") {
      this.pages[this.CurrentPage] = this.htmlContent;
      this.htmlContent = "";

      if (this.pages[this.CurrentPage + 1] != "" &&
        this.pages[this.CurrentPage + 1] != undefined) {
        this.htmlContent = this.pages[this.CurrentPage + 1];
      }
      else {
        this.pages.push("");
      }

      this.CurrentPage++;
    }
  }

  prevPage(): void {
    if (this.htmlContent != "") {
      this.pages[this.CurrentPage] = this.htmlContent;
    }

    if (this.CurrentPage - 1 >= 0) {
      if (this.pages[this.CurrentPage] == "") {
        this.pages.pop();
      }

      this.CurrentPage--;

      this.htmlContent = this.pages[this.CurrentPage];
    }
  }

  removePage(): void {
    if (this.pages.length <= 1) {
      this.htmlContent = "";
    }
    else {
      this.pages.splice(this.CurrentPage, 1)

      this.CurrentPage = 0;
      this.htmlContent = this.pages[this.CurrentPage];
    }
  }

  publishPages(event: any): void {
    this.profanityError = false;

    if (this.htmlContent != "") {
      this.pages[this.CurrentPage] = this.htmlContent;
    }

    let storyPages = {
      storyId: this.StoryId,
      content: this.pages
    }

    this.service.updateStoryPages(storyPages).subscribe(
      data => {
        this.router.navigateByUrl('story/info/' + this.StoryId);
      },
      error => {
        if (error.error.page !== null && error.error.page !== undefined && error.error.page >= 0)
        {
          this.CurrentPage = error.error.page;
          this.htmlContent = this.pages[this.CurrentPage];
          this.profanityError = true;
        }
      });
  }

  cancel(): void {
    this.router.navigateByUrl('');
  }
}
