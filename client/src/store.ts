import { combineReducers, Store as ReduxStore } from 'redux';

export interface Store extends ReduxStore<Store> {}

export default combineReducers({});
