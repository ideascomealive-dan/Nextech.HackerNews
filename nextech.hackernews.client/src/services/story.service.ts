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
