import React, { Component } from 'react'
//import '../styles/notfound.css'

export class NotFound extends Component {

    backgroundColor = {
        backgroundColor: '#444444'
    }

    styling = {
        margin: '0',
        margin: 'auto',
        textAlign: 'center'
    }

    h1Style = {
        color: 'white'
    }
    componentDidMount() {
        document.body.style.backgroundColor = '#444444'
    }

    render() {


        console.log('wtaf')
        return (
            <center>Not found.</center>
        )


    }
}

export default NotFound
