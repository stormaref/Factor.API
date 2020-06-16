import React, { Component } from 'react'
import axios from 'axios'

export class Combo extends Component {



    state = {
        product : '' , 
        no : '' , 
        value : ''  ,
        total : '' ,
        fetched : false ,
        items : [] , 
        selectedItem : ''
    }


//     async componentDidMount  ()  {
        

//         if(this.state.fetched == false)
//         {
//         this.setState({fetched : true})
//         await axios.get("http://app.bazarsefid.com/api/Administrator/GetProducts" , { headers :{
//             Authorization : 'Bearer ' + sessionStorage.getItem('token')
//     }}).then(Response => {
//             Response.data.map((item) => {
//                 const title = item.title
//                 document.getElementById('items').innerHTML +=(`<option value = '${title}'>${title}</option>`)
//                 this.setState({fetched : true})
//              })
            
//             })
//         }
//  }


    show = (item , number , value) => {
        
        this.setState({total : this.state.value *  this.state.no})
        

    }


    onChangeV = (e) => {
        this.setState({value :e.target.value})
    }

    onChangeN = (e) => {
        this.setState({no :e.target.value})
        console.log(this.state.no)
    }



    selectedItem = (e) => {
         this.setState({selectedItem : e.target.value}) 
    }




    setProp = () => {
        this.setState({total : this.state.value *  this.state.no})
        this.props.setItem(this.state.no , this.state.value , this.state.total , this.state.selectedItem , this.props.data)
    }




    async componentDidMount () {
        let option = []
        if (!this.state.fetched)
        {
        this.setState({fetched : true})
        await axios.get("http://app.bazarsefid.com/api/Administrator/GetProducts" , { headers :{
            Authorization : 'Bearer ' + sessionStorage.getItem('token')
    }}).then(Response => {
                 let pre = this.state.items
                pre = pre.concat(<option ></option>)
                this.setState({items : pre})
            Response.data.map((item) => {
                const title = item.title
                pre = pre.concat(<option value={title}>{title}</option>)
                this.setState({items : pre})
                
             })
                this.setState({fetched : true})
            })
            
    }
    }

    


    render() {
     
        return (
            
            <div>
                 <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.13.0/css/all.css" integrity="sha384-Bfad6CLCknfcloXFOyFnlgtENryhrpZCe29RTifKEixXQZ38WheV+i/6YWSzkz3V" crossorigin="anonymous" type="text/css"></link>

<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" integrity="sha384-9aIt2nRpC12Uk9gS9baDl411NQApFmC26EwAOH8WgZl5MYYxFfc+NcPb1dKGj7Sk" crossorigin="anonymous"></link>
                <div className="card">
                    <div className="card-body">
                        <div className = 'row'>
                            <div className= "col">
                                <label for='total'>Total: </label> 
                                <input id = "total" name = "total" value = {this.state.total} onClick = {this.setProp} className = "form-control" readOnly></input>
                            </div>
                            <div className = "col">
                                <label for= "value" >value: </label>
                                <input onChange = {this.onChangeV} id = 'value' name = "value" value = {this.state.value}  className = "form-control" ></input>
                            </div>
                            <div className = "col">
                                <label for= "No" >No: </label>
                                <input id = "No" name = "No" id = "No" onChange = {this.onChangeN}  value = {this.state.no} className = "form-control"></input>
                            </div>
                            <div className= "col">
                                <label for= "select" >Item: </label>
                                <select name = "select" id ='items' className = "form-control" onChange = {this.selectedItem} >
                                    {this.state.items}
                                </select>
                            </div>   
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default Combo


