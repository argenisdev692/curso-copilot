import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Components
import { ButtonComponent } from './components/ui/button/button.component';
import { InputComponent } from './components/ui/input/input.component';
import { LoadingComponent } from './components/feedback/loading/loading.component';
import { HeaderComponent } from './components/layout/header/header.component';
import { SidebarComponent } from './components/layout/sidebar/sidebar.component';

// Directives
import { AutofocusDirective } from './directives/autofocus.directive';

// Pipes
import { DateFormatPipe } from './pipes/date-format.pipe';

@NgModule({
  declarations: [
    ButtonComponent,
    InputComponent,
    LoadingComponent,
    HeaderComponent,
    SidebarComponent,
    AutofocusDirective,
    DateFormatPipe
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule
  ],
  exports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    ButtonComponent,
    InputComponent,
    LoadingComponent,
    HeaderComponent,
    SidebarComponent,
    AutofocusDirective,
    DateFormatPipe
  ]
})
export class SharedModule { }
