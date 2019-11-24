import React from 'react';
import ReduxToastr from 'react-redux-toastr';
import { Route, Switch, BrowserRouter as Router } from 'react-router-dom';


import Login from './pages/Login';
import Register from './pages/Register';
import Account from './pages/Account';

import GenericNotFound from './pages/GenericNotFound';

import saga from './sagas';
import store from './store';
import { Provider } from 'react-redux';
import { sagaMiddleware } from './middleware/sagaMiddleware';

const App = props => (
  <Provider store={store}>
    <Router>
      <>
        <Switch>
          <Route path="/login" component={Login} />
          <Route path="/register" component={Register} />
          <Route path="/account" component={Account} />

          <Route component={GenericNotFound} />
        </Switch>
        <ReduxToastr closeOnToastrClick={true} progressBar={true} />
      </>
    </Router>
  </Provider>
);

export default App;
sagaMiddleware.run(saga);
