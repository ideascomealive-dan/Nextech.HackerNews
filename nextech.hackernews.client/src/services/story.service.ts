import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Story } from "../models/story.model";

@Injectable({ providedIn: 'root' })
export class StoryService {
  constructor(private http: HttpClient) { }

  getStories(page = 1, pageSize = 10, search = ''): Observable<Story[]> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize)
      .set('search', search);

    return this.http.get<Story[]>('/api/stories', { params });
  }
}
