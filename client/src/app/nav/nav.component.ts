import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import {BsDropdownModule} from 'ngx-bootstrap/dropdown'
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  toastrService = inject(ToastrService);
  logginIn = false;
  model: any = {};
  login(){
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        this.logginIn = true;
      },
      error: error => this.toastrService.error(error.error)
    })
  }

  logout(){
    this.logginIn = false;
  }
}
