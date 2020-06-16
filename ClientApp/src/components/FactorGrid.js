import React, { Component } from 'react'

import Factors from './Factors'

export class FactorGrid extends Component {
    state = {
        data : []
    }


    componentDidMount(){
        
         console.log(this.props.factors)
        
    }

    render() {
        
       
        return this.props.factors.map((factor)=>(
            <Factors  data = {factor.id}  showFactor = {this.props.showFactor} ></Factors>
        ))
    }
}

export default FactorGrid
