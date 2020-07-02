import React, {Component} from 'react'
import classNames from 'classnames';
import '../styles/aref.css'

export class Factors extends Component {

    state = {
        data: []
    }

    onClick = (e) => {
        this.props.showFactor(this.props.data)
        this.setState({active: true})
    }

    render() {
        var liClass = classNames({'list-group-item': true, 'active': this.state.active})
        console.log(this.props.data)
        return (
            <div>
                <li onClick={
                        this.onClick
                    }
                    className={liClass}
                    style={
                        {cursor: "pointer"}
                }>
                    {
                    this.props.data != null ? this.props.data : 'No Title'
                }</li>

            </div>
        )
    }
}


export default Factors
