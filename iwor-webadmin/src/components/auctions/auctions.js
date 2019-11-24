import React from 'react';

import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';

import { connectTo } from '../../utils';
import { withStyles } from '@material-ui/core/styles';

import {
  getAuctions,
  deleteAuction,
  closeAuction
} from '../../actions/auctions';

const auctionComparator = (a, b) => {
  if (a.status > b.status) return -1;
  if (a.status < b.status) return 1;

  return 0;
}

class AuctionsPage extends React.Component {
  componentDidMount() {
    this.props.getAuctions();
  }

  render() {
    const { classes, auctions } = this.props;

    return (
      <div className={classes.root}>
        {auctions &&
          auctions
            .sort(auctionComparator)
            .map((a, i) => (
              <ExpansionPanel key={i}>
                <ExpansionPanelSummary
                  expandIcon={<ExpandMoreIcon />}
                  aria-controls="panel1a-content"
                  id="panel1a-header"
                >
                  <Typography
                    className={classes.heading}
                  >{`[${a.status}] ${a.name}`}</Typography>
                </ExpansionPanelSummary>
                <ExpansionPanelDetails
                  style={{ display: 'flex', flexDirection: 'column' }}
                >
                  <Typography>{`Id: ${a.id}`}</Typography>
                  <Typography>{`Description: ${a.description}`}</Typography>
                  <Typography>{`Start Date: ${a.startDate}`}</Typography>
                  <Typography>{`End Date: ${a.endDate}`}</Typography>
                  <Typography>{`Owner Id: ${a.ownerId}`}</Typography>
                  <Typography>{`Start Price: ${a.startPrice}`}</Typography>
                  <Typography>{`Current Price: ${a.currentPrice}`}</Typography>
                  <div>
                    {a.status == 'Open' && (
                      <Button onClick={e => this.props.closeAuction(a.id)}>
                        Close
                      </Button>
                    )}
                    <Button onClick={e => this.props.deleteAuction(a.id)}>
                      Delete
                    </Button>
                  </div>
                </ExpansionPanelDetails>
              </ExpansionPanel>
            ))}
      </div>
    );
  }
}

export default connectTo(
  state => ({ auctions: state.auctions.auctions }),
  {
    getAuctions,
    deleteAuction,
    closeAuction
  },
  withStyles(theme => ({
    root: {
      width: '100%'
    },
    heading: {
      fontSize: theme.typography.pxToRem(15),
      fontWeight: theme.typography.fontWeightRegular
    }
  }))(AuctionsPage)
);
