import { createReducer } from 'redux-act';
import * as a from '../actions/auctions';

const DEFAULT_STATE = {
  auctions: []
};

export default createReducer(
  {
    [a.setAuctions]: (state, auctions) => ({ ...state, auctions })
  },
  DEFAULT_STATE
);
