import { createAction } from 'redux-act';

export const getAuctions = createAction('getAuctions');
export const setAuctions = createAction('setAuctions');

export const closeAuction = createAction('closeAuction');
export const deleteAuction = createAction('deleteAuction');