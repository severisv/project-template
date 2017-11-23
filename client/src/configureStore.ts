import { applyMiddleware, compose, createStore } from 'redux';
import { createLogger } from 'redux-logger';
import ReduxThunk from 'redux-thunk';

import rootReducer from 'store';

declare var __DEV__: boolean;

export default function configureStore(initialState = {}) {
  const middleware = applyMiddleware(
    ReduxThunk,
    createLogger({
      predicate: () => __DEV__
    })
  );

  const store = createStore(
    rootReducer,
    initialState,
    compose(
      middleware,
      __DEV__ && window.devToolsExtension ? window.devToolsExtension() : (f: any) => f
    )
  );

  return store;
}
