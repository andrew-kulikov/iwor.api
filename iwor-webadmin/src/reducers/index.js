import auth from './auth';
import home from './home';
import account from './account';
import client from './client';
import notifications from './notifications'
import { reducer as toastr } from 'react-redux-toastr';

export default {
  auth,
  home,
  account,
  toastr,
  client,
  notifications,
};
