import { put } from 'redux-saga/effects';

import { AUCTIONS, AUCTION } from '../constants/api';

import { setAuctions } from '../actions/auctions';

import { callHttp } from '../utils/api';
import { get, del } from '../utils/httpUtil';

export function* getAuctions() {
  const auctions = yield callHttp(get, AUCTIONS);
  console.log(auctions);
  yield put(setAuctions(auctions));
}

export function* deleteAuction({ payload }) {
  const id = payload;
  yield callHttp(del, AUCTIONS, id);
  yield getAuctions();
}
