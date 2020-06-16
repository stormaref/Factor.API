import React, { Component } from 'react'

import Login from './components/Login'

import Dashboard from './components/Dashboard'

import Code from './components/Code'


import { Switch , BrowserRouter as Router , Route  , Redirect , Link} from 'react-router-dom'
import PrivateRoute from './components/PrivateRoute'







export class App extends Component {
  state = {
    Granted : false , 
    
  }

  

  loginGranted = (Granted) => 
{
  this.setState({Granted : Granted})
}
  
componentDidMount ()
{
  
}
  render() {
    return (
      <Router>
          <Switch>
            <Route exact path = "/" render = {props => (
              <React.Fragment>
               
                <Login loginGranted = {this.loginGranted} ></Login>
              </React.Fragment>
            )}/>
            <Route exact path = "/Code" render = {props => (
              <React.Fragment>
               
                <Code ></Code>
              </React.Fragment>
            )}/>
            <PrivateRoute exact component = {Dashboard} path='/dashboard' />
          </Switch>
      </Router>
    )
  }
}

export default App
