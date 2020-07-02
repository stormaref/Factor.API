import React, { Component } from 'react'

import Login from './components/Login'

import Dashboard from './components/Dashboard'

import Code from './components/Code'

import CommingSoon from './components/comingSoon'

import { Switch, BrowserRouter as Router, Route, Redirect, Link } from 'react-router-dom'
import PrivateRoute from './components/PrivateRoute'
import NotFound from './components/NotFound'







export class App extends Component {
    state = {
        Granted: false,

    }



    loginGranted = (Granted) => {
        this.setState({ Granted: Granted })
    }

    componentDidMount() {

    }
    render() {
        return (
            <Router>
                <Switch>
                    <Route exact path="/" render={props => (
                        <React.Fragment>

                            <CommingSoon></CommingSoon>
                        </React.Fragment>
                    )} />
                    <Route exact path="/Home" render={props => (
                        <React.Fragment>

                            <CommingSoon></CommingSoon>
                        </React.Fragment>
                    )} />
                    <Route exact path="/Admin/Login" render={props => (
                        <React.Fragment>

                            <Login loginGranted={this.loginGranted} ></Login>
                        </React.Fragment>
                    )} />
                    <Route exact path="/Admin/Code" render={props => (
                        <React.Fragment>

                            <Code ></Code>
                        </React.Fragment>
                    )} />
                    <PrivateRoute exact path='/Admin/dashboard' component={Dashboard} />
                    <Route path="*" component={NotFound} />
                </Switch>
            </Router>
        )
    }
}

export default App
