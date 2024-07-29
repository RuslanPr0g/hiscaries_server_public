import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TestsharedService } from '../testshared.service';
import { formatDate } from '@angular/common';
import * as _ from 'lodash';
import { Genre } from '../publish/Genre';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
})
export class AdminPanelComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private service: TestsharedService,
    private router: Router
  ) {
    this.formuserdata = this.formBuilder.group({
      email: '',
      dob: '',
    });

    this.modifystory = this.formBuilder.group({
      storyId: '',
      title: '',
      description: '',
      authorname: '',
      genreId: '',
      agelimit: '',
      datewritten: new Date(),
    });

    this.formgenredata = this.formBuilder.group({
      name: '',
      description: '',
    });

    this.formreportdata = this.formBuilder.group({
      storyId: '',
      page: '',
      status: '',
      dateAdded: '',
    });
  }

  modifystory: FormGroup;
  formuserdata: FormGroup;
  formgenredata: FormGroup;
  formreportdata: FormGroup;

  ngOnInit(): void {
    if (this.service.isAuthenticated() === false) {
      this.router.navigateByUrl('login');
    }

    this.getUsers();
    this.getStories();
    this.getGenres();
    this.getUserInfo();
    this.setGenres();
    this.setReports();
  }

  displayedColumnsUser: string[] = [
    'id',
    'username',
    'email',
    'birthDate',
    'accountCreated',
    'banned',
  ];
  displayedColumnsStory: string[] = [
    'id',
    'title',
    'description',
    'written',
    'published',
    'author',
    'publisher',
  ];
  displayedColumnsGenre: string[] = ['id', 'name', 'description'];
  displayedColumnsReport: string[] = ['storyId', 'page', 'status', 'dateAdded'];

  storyData: any[] = [];
  userData: any[] = [];
  genreData: any[] = [];

  clickedStoryData: any = {};
  clickedUserData: any = {};
  clickedGenreData: any = {};
  clickedReportData: any = {};

  showStoryModal: boolean = false;
  showUserModal: boolean = false;
  showGenreModal: boolean = false;

  isClosedReportsShowed: boolean = false;
  reports: any = [];
  reportsData: any = [];

  currentTab: number = 1;

  User: any = {};

  IsError: boolean = false;
  IsLoading: boolean = true;
  errorMessage: string = '';

  genres: Genre[] = [];
  GenreList: any = [];

  fileName = '';

  selectedValueStoryGenre: number[] = [];

  StoryId: any = 0;

  clickUsers(row: any): any {
    this.clickedUserData = row;
    this.showUserModal = true;

    this.formuserdata.controls['email'].setValue(this.clickedUserData.email);
    this.formuserdata.controls['dob'].setValue(
      formatDate(this.clickedUserData.birthDate, 'yyyy-MM-dd', 'en')
    );
  }

  clickStories(row: any): any {
    this.clickedStoryData = row;

    this.StoryId = row.id;

    this.selectedValueStoryGenre = this.clickedStoryData.genres.map(
      (x: any) => {
        return x.id;
      }
    );

    this.modifystory = this.formBuilder.group({
      title: this.clickedStoryData.title,
      description: this.clickedStoryData.description,
      authorname: this.clickedStoryData.authorName,
      agelimit: this.clickedStoryData.ageLimit,
      datewritten: this.clickedStoryData.dateWritten,
    });

    this.showStoryModal = true;
  }

  toAddGenre: boolean = false;

  showReportModal: boolean = false;

  ReportedStoryPage: any = {};

  clickGenres(row: any): any {
    this.toAddGenre = false;
    this.clickedGenreData = row;
    this.showGenreModal = true;

    this.formgenredata = this.formBuilder.group({
      name: this.clickedGenreData.name,
      description: this.clickedGenreData.description,
    });
  }

  clickReport(row: any): any {
    if (row.status == 'InReview') {
      this.service.getPagesForStory({ id: row.storyId }).subscribe((data) => {
        this.ReportedStoryPage = data.filter(
          (p: any) => p.page == row.page
        )[0].content;

        this.clickedReportData = row;
        this.showReportModal = true;

        this.formreportdata = this.formBuilder.group({
          storyId: this.clickedReportData.storyId,
          page: this.clickedReportData.page,
          status: this.clickedReportData.status,
          dateAdded: this.clickedReportData.dateAdded,
        });
      });
    }
  }

  showPendingReports() {
    const inreviewStatus = 0;

    this.reportsData = this.reports.filter((r: any) => {
      return r.status == inreviewStatus;
    });

    this.mapReports();

    this.isClosedReportsShowed = false;
    this.showReportModal = false;
  }

  getStatus = (s: any) => {
    if (s == 0) return 'InReview';
    else if (s == 1) return 'Declined';
    else if (s == 2) return 'Approved';
    else return 'Unknown';
  };

  mapReports() {
    this.reportsData = this.reportsData.map((r: any) => {
      return {
        id: r.id,
        storyId: r.storyId,
        page: r.page,
        status: this.getStatus(r.status),
        reporterId: r.reporterId,
        dateAdded: r.dateAdded,
      };
    });
  }

  showClosedReports() {
    const inreviewStatus = 0;

    this.reportsData = this.reports.filter((r: any) => {
      return r.status != inreviewStatus;
    });

    this.mapReports();

    this.isClosedReportsShowed = true;
  }

  setReports(): void {
    this.service.getStoryReports().subscribe((data) => {
      this.reports = data;

      this.showPendingReports();
    });
  }

  declineReport(): any {
    this.service
      .storyReportDecline({
        storyId: this.clickedReportData.storyId,
        page: this.clickedReportData.page,
      })
      .subscribe((data) => {
        this.setReports();
      });
  }

  approveReportData(): any {
    this.service
      .storyReportApprove({
        storyId: this.clickedReportData.storyId,
        page: this.clickedReportData.page,
      })
      .subscribe((data) => {
        this.setReports();
      });
  }

  addGenreModal(): any {
    this.toAddGenre = true;
    this.showGenreModal = true;
  }

  changeTab(tab: number): any {
    if (tab > 0 && tab <= 4) {
      this.currentTab = tab;
    }
  }

  getUsers(): any {
    this.service.getUsers().subscribe((data) => {
      this.userData = data.filter((x: any) => x.userrole !== 'admin');
    });
  }

  getStories(): any {
    this.service.getStories().subscribe((data) => {
      this.storyData = data;
    });
  }

  getGenres(): any {
    this.service.getGenres().subscribe((data) => {
      this.genreData = data;

      this.formgenredata = this.formBuilder.group({
        name: data.name,
        description: data.description,
      });
    });
  }

  validateEmail(email: string) {
    return String(email)
      .toLowerCase()
      .match(
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      );
  }

  updateUserData(banned: boolean): void {
    var email = this.formuserdata.getRawValue().email;

    if (!this.validateEmail(email)) {
      this.errorMessage = "It's not an email!";
      return;
    } else {
      this.errorMessage = '';
    }

    this.service
      .updateUsersData({
        email: email,
        birthDate: this.formuserdata.getRawValue().dob,
        banned: banned,
        usernameUpdate: this.clickedUserData.username,
      })
      .subscribe(
        (data) => {
          this.getUsers();
          this.showUserModal = false;
        },
        (error) => {
          console.log(error);
          this.errorMessage = error.error;

          setTimeout(() => {
            this.errorMessage = '';
          }, 6000);
        }
      );
  }

  setGenres(): void {
    this.service.getGenres().subscribe((data) => {
      this.GenreList = data.reverse();

      this.GenreList.forEach((genre: any) => {
        this.genres.push({ id: genre.id, name: genre.name });
      });
    });
  }

  modifyStoryInfo(): void {
    if (this.selectedValueStoryGenre.length === 0) {
      alert('Select at leat one genre!');
      return;
    }

    var age = this.modifystory.controls['agelimit'].value;

    if (+age <= 0) {
      this.errorMessage = 'Please, enter valid age!';
      return;
    }

    let storyMod = this.modifystory.getRawValue();
    storyMod['storyId'] = this.StoryId;
    storyMod['imagePreview'] = this.cardImageBase64;
    storyMod['genreIds'] = this.selectedValueStoryGenre;

    this.service.updateStoryInfo(storyMod).subscribe(
      (data) => {
        this.getStories();
        this.showStoryModal = false;
      },
      (error) => {
        console.error('Modify Error', error);
        let errorstring = '';

        for (const [key, value] of Object.entries(error.error.errors)) {
          errorstring += `${key}: ${value}`;
        }

        if (errorstring.includes('agelimit')) {
          this.errorMessage = 'Please, enter valid age!';
          return;
        } else {
          alert(errorstring);
        }

        this.errorMessage = error.error;
      }
    );
  }

  imageError: any;
  isImageSaved: boolean = false;
  cardImageBase64: any;

  uploadimage(fileInput: any) {
    this.imageError = null;
    if (fileInput.target.files && fileInput.target.files[0]) {
      // Size Filter Bytes
      const max_size = 20971520;
      const allowed_types = ['image/png', 'image/jpeg'];
      const max_height = 15200;
      const max_width = 25600;

      if (fileInput.target.files[0].size > max_size) {
        this.imageError = 'Maximum size allowed is ' + max_size / 1000 + 'Mb';

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
        image.onload = (rs) => {
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

  deleteStoryData() {
    this.service.deleteStory({ storyId: this.StoryId }).subscribe(
      (data) => {
        this.getStories();
        this.showStoryModal = false;
      },
      (error) => {
        console.log(error);
        this.IsError = true;
        this.errorMessage = error.error;
      }
    );
  }

  addGenre(genre: any): any {
    this.service.addGenre(genre).subscribe(
      (data) => {
        this.showGenreModal = false;
        this.getGenres();
      },
      (error) => {
        alert('Please, provide clear title and description!');
      }
    );
  }

  updateGenreData(): any {
    var genre = this.formgenredata.getRawValue();

    genre['id'] = this.clickedGenreData.id;

    if (this.toAddGenre) {
      this.addGenre(genre);
      this.getGenres();
      return;
    }

    this.service.editGenre(genre).subscribe((data) => {
      this.showGenreModal = false;
      this.getGenres();
    });
  }

  deleteGenreData(): any {
    this.service.deleteGenre(this.clickedGenreData).subscribe(
      (data) => {
        this.getGenres();
        this.showGenreModal = false;
      },
      (error) => {
        console.log(error);
        this.IsError = true;
        this.errorMessage = error.error;
      }
    );
  }

  getUserInfo(): any {
    this.service.getUserInfo().subscribe(
      (data) => {
        this.User = data;
        let isError =
          this.User == null || this.User == undefined ? true : false;

        if (this.User.role !== 'admin') {
          this.router.navigateByUrl('');
        }

        if (isError) {
          setTimeout(() => {
            this.IsError = true;
          }, 1000);
        }

        setTimeout(() => {
          this.IsLoading = false;
        }, 1000);
      },
      (error) => {
        this.IsError = true;
      }
    );
  }
}
