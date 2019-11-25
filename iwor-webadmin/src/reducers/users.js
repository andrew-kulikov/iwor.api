import { createReducer } from 'redux-act';
import * as a from '../actions/users';

const DEFAULT_STATE = {
  users: []
};

export default createReducer(
  {
    [a.setUsers]: (state, users) => ({ ...state, users })
  },
  DEFAULT_STATE
);
