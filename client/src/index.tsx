import 'font-awesome/css/font-awesome.css';
import 'less/base.less';
import 'react-select/dist/react-select.css';
import 'react-table/react-table.css';

import * as React from 'react';
import * as ReactDOM from 'react-dom';

import { AppContainer } from 'react-hot-loader';
import { Provider } from 'react-redux';
import App from './App';
import configureStore from './configureStore';

declare var module: { hot: any };

const root = document.getElementById('root');
const store = configureStore();

if (module.hot) {
  // Enable Webpack hot module replacement for reducers
  module.hot.accept('./store', () => {
    const nextRootReducer = require('./store');
    store.replaceReducer(nextRootReducer);
  });
}

const render = (Component: any) => {
  ReactDOM.render(
    <AppContainer>
      <Provider store={store}>
        <Component />
      </Provider>
    </AppContainer>,
    root
  );
};

render(App);

// Hot Module Replacement API
if (module.hot) {
  module.hot.accept();
}
