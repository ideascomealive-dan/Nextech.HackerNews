import { Component, OnInit } from "@angular/core";
import { Story } from "../../models/story.model";
import { StoryService } from "../../services/story.service";
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';


@Component({
  selector: 'app-story-list',
  standalone: true,
  templateUrl: './story-list.component.html',
  styleUrls: ['./story-list.component.css'],
  imports: [
    CommonModule,
    FormsModule
  ],
})
export class StoryListComponent implements OnInit {
  stories: Story[] = [];
  search = '';
  page = 1;
  pageSize = 10;

  constructor(private storyService: StoryService) { }

  private searchSubject = new Subject<string>();

  ngOnInit(): void {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(term => {
      this.search = term;
      this.page = 1;
      this.loadStories();
    });

    this.loadStories();
  }

  isLoading = false;

  loadStories(): void {
    this.isLoading = true;
    this.storyService.getStories(this.page, this.pageSize, this.search)
      .subscribe((data: Story[]) => {
        this.stories = data;
        this.isLoading = false;
      });
  }

  onSearchChange(): void {
    this.searchSubject.next(this.search);
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
