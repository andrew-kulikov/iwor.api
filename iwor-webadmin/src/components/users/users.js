import React from 'react';

import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';

import { connectTo } from '../../utils';
import { withStyles } from '@material-ui/core/styles';

import { getUsers, deleteUser } from '../../actions/users';

class UsersPage extends React.Component {
  componentDidMount() {
    this.props.getUsers();
  }

  render() {
    const { classes, users } = this.props;

    return (
      <div className={classes.root}>
        {users &&
          users.map((u, i) => (
            <ExpansionPanel key={i}>
              <ExpansionPanelSummary
                expandIcon={<ExpandMoreIcon />}
                aria-controls="panel1a-content"
                id="panel1a-header"
              >
                <Typography
                  className={classes.heading}
                >{`${u.username} : ${u.email}`}</Typography>
              </ExpansionPanelSummary>
              <ExpansionPanelDetails
                style={{ display: 'flex', flexDirection: 'column' }}
              >
                <Typography>{`Id: ${u.id}`}</Typography>
                <Typography>{`Fist Name: ${u.firstName}`}</Typography>
                <Typography>{`Last Name: ${u.lastName}`}</Typography>
                <Typography>{`Birthday: ${u.birthday}`}</Typography>
                <Typography>{`Phone Number: ${u.phoneNumber}`}</Typography>
                <Typography>{`Address: ${u.address}`}</Typography>
                <Typography>{`Registration Date: ${u.registrationDate}`}</Typography>

                <Button onClick={e => this.props.deleteUser(u.id)}>
                  Delete
                </Button>
              </ExpansionPanelDetails>
            </ExpansionPanel>
          ))}
      </div>
    );
  }
}

export default connectTo(
  state => ({ users: state.users.users }),
  {
    getUsers,
    deleteUser
  },
  withStyles(theme => ({
    root: {
      width: '100%'
    },
    heading: {
      fontSize: theme.typography.pxToRem(15),
      fontWeight: theme.typography.fontWeightRegular
    }
  }))(UsersPage)
);
