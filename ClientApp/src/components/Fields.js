import React, { Component } from 'react'
import Combo from './Combo'

export class Fields extends Component {
    render() {
        return (
            this.props.Items.map((item) => (
                <Combo data = {item}  setItem = {this.props.setItem} bool = {this.props.bool} ></Combo>
            ))
        )
    }
}

export default Fields
