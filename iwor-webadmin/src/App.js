import React from 'react';
import ReduxToastr from 'react-redux-toastr';
import { Route, Switch, BrowserRouter as Router } from 'react-router-dom';

import Login from './pages/Login';
import Home from './pages/Home';
import Auctions from './pages/Auctions';

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
          <Route path="/auctions" component={Auctions} />
          <Route path="/login" component={Login} />

          <Route path="/" component={Home} />

          <Route component={GenericNotFound} />
        </Switch>
        <ReduxToastr closeOnToastrClick={true} progressBar={true} />
      </>
    </Router>
  </Provider>
);

export default App;
sagaMiddleware.run(saga);
