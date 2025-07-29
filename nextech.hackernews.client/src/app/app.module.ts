import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StoryListComponent } from '../components/story-list/story-list.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', component: StoryListComponent },  // Default route
  { path: '**', redirectTo: '' }                // Redirect unknown routes to home
];

@NgModule({
  declarations: [
    AppComponent    
  ],
  imports: [
    BrowserModule, FormsModule, HttpClientModule,
    AppRoutingModule, StoryListComponent, RouterModule.forRoot(routes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
