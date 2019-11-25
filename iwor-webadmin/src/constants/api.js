const BACKEND = process.env.REACT_APP_API_URL;
const API = BACKEND + 'api';

export const LOGIN = `${API}/auth/token`;

export const AUCTIONS = `${API}/auctions`;
export const AUCTIONS_OPEN = `${API}/auctions/open`;
export const AUCTION = id => `${API}/auctions/${id}`;

export const USERS = `${API}/users`;

export const ME = `${API}/me`;
