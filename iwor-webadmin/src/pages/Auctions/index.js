import React from 'react';

import styles from './style';
import Page from '../page';

import Typography from '@material-ui/core/Typography';

import { withStyles } from '@material-ui/core/styles';

import Auctions from '../../components/auctions/auctions';

const AuctionsPage = ({ classes }) => (
  <Page title="Auctions">
    <Typography variant="h4" gutterBottom component="h2">
      Auctions
    </Typography>
    <Auctions />
  </Page>
);

export default withStyles(styles)(AuctionsPage);
