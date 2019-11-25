import { put } from 'redux-saga/effects';

import { USERS } from '../constants/api';

import { setUsers } from '../actions/users';

import { callHttp } from '../utils/api';
import { get, del } from '../utils/httpUtil';

import { toastr } from 'react-redux-toastr';
import * as messageTypes from '../constants/messageTypes';

export function* getUsers() {
  const users = yield callHttp(get, USERS);
  console.log(users);
  yield put(setUsers(users));
}

export function* deleteUser({ payload }) {
  const id = payload;
  try {
    yield callHttp(del, USERS, id);
    yield getUsers();
  } catch (e) {
    toastr.error(messageTypes.ERROR, 'Delete user error');
  }
}
