import React, { Component } from 'react'

export class comingSoon extends Component {

    backgroundColor = {
        backgroundColor : '#444444'
    }

    styling = {
        margin : '0' ,
        margin : 'auto' ,
        textAlign : 'center'
    }

    h1Style = {
        color : 'white' 
    }
    componentDidMount () {
        document.body.style.backgroundColor = '#444444'
    }

    render() {


        console.log('wtaf')
        return (
            <div >
                <body style = {this.backgroundColor}>
                    <div style={this.styling}>
                        <h1 style={this.h1Style}>Coming Soon...</h1>
                    </div>
                </body>
            </div>
        )
           
    
    }
}   

export default comingSoon
