import auth from './auth';
import home from './home';
import users from './users';
import auctions from './auctions';
import notifications from './notifications'
import { reducer as toastr } from 'react-redux-toastr';

export default {
  auth,
  home,
  users,
  toastr,
  auctions,
  notifications,
};
