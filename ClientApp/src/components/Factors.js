import React, { Component } from 'react'

export class Factors extends Component {
    
    state = {
        data : []
    }

    onClick = () => {
        this.props.showFactor(this.props.data)
    }

    render() {

      


        console.log(this.props.data)
        return (
            <div>
                <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" integrity="sha384-9aIt2nRpC12Uk9gS9baDl411NQApFmC26EwAOH8WgZl5MYYxFfc+NcPb1dKGj7Sk" crossorigin="anonymous"></link>
                
                    <button type="button" className="btn btn-primary btn-block form-control" onClick = {this.onClick}>{this.props.data}</button>
                
            </div>
        )
    }
}

export default Factors
