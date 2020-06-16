import React, { Component } from 'react'
import axios from 'axios'

export class Test extends Component {

   
    onClicke = async() => {
        console.log('kir')
        const response = await axios.get('http://app.bazarsefid.com/api/Administrator/GetAllFactors')
        console.log(response)
    }

    render() {
      
        



        return (
            <div>
                <input type = "button" onClick = {this.onClicke}>
                </input>
            </div>
        )
    }

}
export default Test
