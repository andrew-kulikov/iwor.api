import { createAction } from 'redux-act';

export const getUsers = createAction('getUsers');
export const setUsers = createAction('setUsers');

export const deleteUser = createAction('deleteUser');