import React, { Component } from 'react'
import Combo from './Combo'

export class Fields extends Component {
    render() {
        return (
            this.props.Items.map((item) => (
                <Combo data = {item}  setItem = {this.props.setItem} ></Combo>
            ))
        )
    }
}

export default Fields
