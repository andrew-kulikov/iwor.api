import { put } from 'redux-saga/effects';

import { ME } from '../constants/api';

import { setMe } from '../actions/account';

import { callHttp } from '../utils/api';
import { get } from '../utils/httpUtil';

export function* getMe() {
  const { user } = yield callHttp(get, ME);
  yield put(setMe(user));
}
