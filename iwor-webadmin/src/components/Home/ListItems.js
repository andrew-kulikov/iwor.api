import React from 'react';

import { Link } from 'react-router-dom';

import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import DashboardIcon from '@material-ui/icons/Dashboard';
import ListAltOutlined from '@material-ui/icons/ListAltOutlined';
import LogoutIcon from '@material-ui/icons/ExitToApp';
import GroupOutlined from '@material-ui/icons/GroupOutlined';

import { logout } from '../../actions/auth';
import { connectTo } from '../../utils';

import { withRouter } from 'react-router-dom';

export const MainListItems = withRouter(props => (
  <div>
    <ListItem component={Link} to="/">
      <ListItemIcon>
        <DashboardIcon />
      </ListItemIcon>
      <ListItemText primary="Dashboard" />
    </ListItem>
    <ListItem component={Link} to="/users">
      <ListItemIcon>
        <GroupOutlined />
      </ListItemIcon>
      <ListItemText primary="Users" />
    </ListItem>
    <ListItem component={Link} to="/auctions">
      <ListItemIcon>
        <ListAltOutlined />
      </ListItemIcon>
      <ListItemText primary="Auctions" />
    </ListItem>
  </div>
));

export const SecondaryListItems = connectTo(
  state => ({}),
  {
    logout
  },
  props => (
    <div>
      <ListItem button onClick={() => props.logout()}>
        <ListItemIcon>
          <LogoutIcon />
        </ListItemIcon>
        <ListItemText primary="Logout" />
      </ListItem>
    </div>
  )
);
