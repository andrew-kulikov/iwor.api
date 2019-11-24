import React from 'react';

import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';

import { connectTo } from '../../utils';
import { withStyles } from '@material-ui/core/styles';
import { makeStyles } from '@material-ui/core/styles';

import { getAuctions, deleteAuction } from '../../actions/auctions';

class AuctionsPage extends React.Component {
  componentDidMount() {
    this.props.getAuctions();
  }

  render() {
    const { classes } = this.props;

    return (
      <div className={classes.root}>
        {this.props.auctions &&
          this.props.auctions.map((a, i) => (
            <ExpansionPanel key={i}>
              <ExpansionPanelSummary
                expandIcon={<ExpandMoreIcon />}
                aria-controls="panel1a-content"
                id="panel1a-header"
              >
                <Typography className={classes.heading}>{a.name}</Typography>
              </ExpansionPanelSummary>
              <ExpansionPanelDetails>
                <Typography>{a.description}</Typography>
                <Button onClick={e => this.props.deleteAuction(a.id)}>Delete</Button>
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
    deleteAuction
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
