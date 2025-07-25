@Component({
  selector: 'app-story-list',
  templateUrl: './story-list.component.html',
})
export class StoryListComponent implements OnInit {
  stories: Story[] = [];
  search = '';
  page = 1;
  pageSize = 10;

  constructor(private storyService: StoryService) { }

  ngOnInit(): void {
    this.loadStories();
  }

  loadStories(): void {
    this.storyService.getStories(this.page, this.pageSize, this.search)
      .subscribe(data => this.stories = data);
  }

  onSearchChange(): void {
    this.page = 1;
    this.loadStories();
  }

  nextPage(): void {
    this.page++;
    this.loadStories();
  }

  prevPage(): void {
    if (this.page > 1) this.page--;
    this.loadStories();
  }
}
