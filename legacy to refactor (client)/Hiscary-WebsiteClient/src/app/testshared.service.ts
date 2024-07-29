import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router, UrlSerializer } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class TestsharedService {
  readonly APIUrl = "https://localhost:5001/api/v1";

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, 
    private router: Router, private serializer: UrlSerializer) {
  }

  //
  // *** STORY ***
  //

  getGenres() {
    return this.http.get<any>(this.APIUrl + "/story/genres",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  addGenre(val: any) {
    return this.http.post<any>(this.APIUrl + "/story/genres", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  editGenre(val: any) {
    return this.http.patch(this.APIUrl + "/story/genre", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  deleteGenre(val: any) {
    return this.http.post(this.APIUrl + "/story/genre/delete", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getHistory() {
    return this.http.get<any>(this.APIUrl + "/story/history",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getStoryList(): Observable<any[]> {
    return this.http.get<any>(this.APIUrl + "/story/shuffle",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  addStory(val: any) {
    return this.http.post(this.APIUrl + "/story", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  deleteStory(val: any) {
    return this.http.post(this.APIUrl + "/story/delete", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  readStoryHistory(val: any) {
    return this.http.post(this.APIUrl + "/story/readstory", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getAudio(val: any) {
    return this.http.get<any>(this.APIUrl + "/story/audio/" + val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  deleteAudio(val: any) {
    return this.http.delete(this.APIUrl + "/story/audio/" + val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  addStoryPage(val: any) {
    return this.http.post(this.APIUrl + "/story/page", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  bookmarkStory(val: any) {
    return this.http.post(this.APIUrl + "/story/bookmark", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  writeComment(val: any) {
    return this.http.post(this.APIUrl + "/story/comment", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getStoryReports() {
    return this.http.get<any>(this.APIUrl + "/story/report",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  storyReport(val: any) {
    return this.http.post(this.APIUrl + "/story/report", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  storyReportApprove(val: any) {
    return this.http.post(this.APIUrl + "/story/report/approve", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  storyReportDecline(val: any) {
    return this.http.post(this.APIUrl + "/story/report/decline", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  scoryStore(val: any) {
    return this.http.post(this.APIUrl + "/story/score", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  removeComment(val: any) {
    return this.http.post(this.APIUrl + "/story/comment/force", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  updateStoryInfo(val: any) {
    return this.http.patch(this.APIUrl + "/story/", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getPagesForStory(val: any) {
    return this.http.get<any>(this.APIUrl + "/story/page/" + val.id,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  storyScores() {
    return this.http.get<any>(this.APIUrl + "/story/score",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getComments() {
    return this.http.get<any>(this.APIUrl + "/story/comment",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getBookmarks() {
    return this.http.get<any>(this.APIUrl + "/story/bookmark/",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  search(val: any) {
    const tree = this.router.createUrlTree(["story"], { queryParams: { search: val.search, genre: val.genre } });
    let query = this.serializer.serialize(tree);
    return this.http.get<any>(this.APIUrl + query,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getStoryById(val: any) {
    return this.http.get<any>(this.APIUrl + "/story?id=" + val.id,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getStories() {
    return this.http.get<any>(this.APIUrl + "/story/stories",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  //
  // *** USER ***
  //

  getUsers() {
    return this.http.get<any>(this.APIUrl + "/user/users/",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getReviews() {
    return this.http.get<any>(this.APIUrl + "/user/review/",
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  publishReview(val: any) {
    return this.http.post(this.APIUrl + "/user/review", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  removeReview(val: any) {
    return this.http.post(this.APIUrl + "/user/review/force", val,
    { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  becomePublisher() {
    return this.http.get<any>(this.APIUrl + "/user/becomepublisher",
      { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getUserInfo() {
    return this.http.get<any>(this.APIUrl + "/user",
      { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  getUserInfoUsername(username: string) {
    const tree = this.router.createUrlTree(["user"], { queryParams: { username } });
    let query = this.serializer.serialize(tree);
    return this.http.get<any>(this.APIUrl + query,
      { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  login(val: any) {
    return this.http.post(this.APIUrl + "/user/login", val)
      .pipe(
        tap((tokenData: any) => {
          localStorage.setItem(environment.ACCESS_TOKEN_KEY, tokenData.token);
          localStorage.setItem(environment.REFRESH_TOKEN_KEY, tokenData.refreshToken);
        })
      );
  }

  updateUsersData(val: any) {
    return this.http.patch(this.APIUrl + "/user/update-profile", val,
      { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  updateStoryPages(val: any) {
    return this.http.post(this.APIUrl + "/story/pages", val,
      { headers: { "Authorization": "Bearer " + localStorage.getItem(environment.ACCESS_TOKEN_KEY) } });
  }

  logOut(): void {
    localStorage.removeItem(environment.ACCESS_TOKEN_KEY);
    localStorage.removeItem(environment.REFRESH_TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    let token = localStorage.getItem(environment.ACCESS_TOKEN_KEY);
    let authenticated = token != null && !this.jwtHelper.isTokenExpired(token);
    if (authenticated == false) {
      this.logOut();
      return false;
    }

    return true;
  }

  register(val: any) {
    return this.http.post(this.APIUrl + "/user/register", {
      username: val.username,
      email: val.email,
      birthDate: val.dob,
      password: val.password
    })
      .pipe(
        tap((tokenData: any) => {
          localStorage.setItem(environment.ACCESS_TOKEN_KEY, tokenData.token);
          localStorage.setItem(environment.REFRESH_TOKEN_KEY, tokenData.refreshToken);
        })
      );
  }
}
