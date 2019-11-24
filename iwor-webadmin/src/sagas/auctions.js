import { put } from 'redux-saga/effects';

import { AUCTIONS, AUCTION, AUCTIONS_OPEN } from '../constants/api';

import { setAuctions } from '../actions/auctions';

import { callHttp } from '../utils/api';
import { get, del } from '../utils/httpUtil';

import { toastr } from 'react-redux-toastr';
import * as messageTypes from '../constants/messageTypes';

export function* getAuctions() {
  const auctions = yield callHttp(get, AUCTIONS);
  console.log(auctions);
  yield put(setAuctions(auctions));
}

export function* deleteAuction({ payload }) {
  const id = payload;
  try {
    yield callHttp(del, AUCTIONS, id);
    yield getAuctions();
  } catch (e) {
    toastr.error(messageTypes.ERROR, 'Delete auction error');
  }
}

export function* closeAuction({ payload }) {
  const id = payload;
  try {
    yield callHttp(del, AUCTIONS_OPEN, id);
    yield getAuctions();
  } catch (e) {
    toastr.error(messageTypes.ERROR, 'Close auction error');
  }
}
