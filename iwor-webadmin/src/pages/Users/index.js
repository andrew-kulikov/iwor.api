import React from 'react';

import styles from './style';
import Page from '../page';

import Typography from '@material-ui/core/Typography';

import { withStyles } from '@material-ui/core/styles';

import Users from '../../components/users/users';

const UsersPage = ({ classes }) => (
  <Page title="Auctions">
    <Typography variant="h4" gutterBottom component="h2">
      Users
    </Typography>
    <Users />
  </Page>
);

export default withStyles(styles)(UsersPage);
