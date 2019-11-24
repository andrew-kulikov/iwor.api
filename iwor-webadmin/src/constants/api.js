const BACKEND = process.env.REACT_APP_API_URL;
const API = BACKEND + 'api';

export const LOGIN = `${API}/auth/token`;

export const USERS = `${API}/users`;

export const CLIENT_SENSOR = id => `${API}/sensor/${id}`;

export const ME = `${API}/me`;
