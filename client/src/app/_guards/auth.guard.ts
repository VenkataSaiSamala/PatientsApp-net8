import { Inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = Inject(AccountService);
  const toastrService = Inject(ToastrService);

  if(accountService.currentUser()){

  } else{
    toastrService.error('You shall not pass!!!')
    return false;
  }

  return true;
};
