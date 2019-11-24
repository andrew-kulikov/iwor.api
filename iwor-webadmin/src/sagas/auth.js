import { put } from 'redux-saga/effects';

import { LOGIN } from '../constants/api';

import { loginOk } from '../actions/auth';

import { callHttp } from '../utils/api';
import { post } from '../utils/httpUtil';
import { toastr } from 'react-redux-toastr';
import * as messageTypes from '../constants/messageTypes';

export function* login({ payload }) {
    const { username, password } = payload;
    try {
        const data = yield callHttp(post, LOGIN, { username, password });

        yield put(
            loginOk({
                token: data.token,
                refreshToken: data.refresh_tokens
            })
        );
    } catch (err) {
        toastr.error(messageTypes.ERROR, 'Authorization failed');
    }
}
