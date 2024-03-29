import { call, put } from 'redux-saga/effects';
import { logout } from '../actions/auth';
import jwt_decode from 'jwt-decode';

export function* callHttp(...args) {
  try {
    const token = localStorage.getItem('token');

    if (token) {
      let decoded;
      try {
        decoded = jwt_decode(token);
      } catch (error) {
        throw new Error(`Invalid token ${token}`);
      }

      if (
        decoded &&
        typeof decoded.exp !== 'undefined' &&
        decoded.exp < new Date().getTime() / 1000
      ) {
        throw new Error('401 Forbidden');
      }
    }

    const data = yield call(...args);
    
    return data.result;
  } catch (err) {
    if (err.status === 401) {
      console.log(err);
      yield put(logout());
    }
    throw err;
  }
}
