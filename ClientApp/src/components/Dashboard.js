import React, { Component } from 'react'


import axios from 'axios'

import FactorGrid from './FactorGrid'

import ImageGrid from './ImageGrid'

import Fields from './Fields'


import { DatePicker } from "jalali-react-datepicker";

import Combo from './Combo'

export class Dashboard extends Component {

    state = {
        users :[],
        selectedUserPhone : '' , 
        usersOption : [] ,
        images : [
        ],
        factors : [],
        loading : false,
        items  : [],
        makeFactor : false ,
        factorItem : [] ,
        toBeSent : [],
        endOfFactor : false ,
        selectedPreFactor : [],
        contacts : [],
        contactOptions: [],
        date : '', 
        selectedContact : '' ,
        factorData : {
            
            factorItems : [
                
            ],
            preFactorId : '',
            state: {
                isClear: true
              },
              contactId : '',
              date : ''
        },
        endOfFactorClicked : false

    }

    onSubmit = async(e)=>
        {
            e.preventDefault()
            
            

            // response.data.map((image) => (
            //     this.state.images.push(image)
            // ))

            //console.log(this.state.images)

            
        }





        

        componentDidMount() {

            let data = this.state.users
            
            let options = this.state.usersOption
            options.push(<option></option>)
            axios.get('http://app.bazarsefid.com/api/Administrator/GetAllUsers' , {headers : {
                Authorization : 'Bearer ' + sessionStorage.getItem('token')
            }}).then(response => {
                console.log(response.data)
                response.data.map((user) => (
                    options.push(<option value = {user.phone}>{user.phone}</option>)
                ))
                this.setState({usersOption : options})

                response.data.map((user) => (
                    data.push(user)
                ))
                console.log(data)
                this.setState({users : data})
            })
        }


        showFactor = (data) =>
        {
            const factors = this.state.factors
            let no = 0

            for (let index = 0; index < factors.length; index++) {
                if(factors[index].id == data)
                {
                    no = index
                    break ;
                }
                
            }
            
            
            this.setState({selectedPreFactor : factors[no].id})


            this.setState({images : factors[no].images})

        }


        fetchFactor = () => 
        {
            console.log(this.state.selectedUserPhone)
            this.setState({ loading : true})
            const response = axios.get('http://app.bazarsefid.com/api/Administrator/GetUserUndoneFactors' , { params : {
                phone : this.state.selectedUserPhone
            } ,
                headers :{
            Authorization : 'Bearer ' + sessionStorage.getItem('token')
            }}).then(response => {
                console.log(response.data)
                
                
               this.setState({factors :response.data })



                this.setState({loading : false})

                

                
                

                
                
                
                
                
            })

        }





        setItem = (no , value , total , item , data) => {
           console.log(no)
           console.log(value)
           console.log(no*value)
           console.log(item)
           console.log(data)
           console.log(this.state.selectedPreFactor)
           console.log(this.state.date)
           console.log(this.state.selectedContact)
            let Data = this.state.factorData
            Data.preFactorId = this.state.selectedPreFactor

            Data.factorItems.push( {
                product : {
                    title : item

                },
                quantity :Number (no) , 
                price : Number (value)
            })

            this.setState({factorData : Data})

            

            
        }





        Box = async () => {
            this.setState({makeFactor : true})
            let prev = this.state.factorItem
            prev = prev.concat(Date.now())
            this.setState({factorItem : prev })
            this.setState({endOfFactor : true})

            

             let List = [<option></option>]

           await axios.get('http://app.bazarsefid.com/api/Administrator/GetUserContacts' , { params :  {
                phone : this.state.selectedUserPhone
             },
             headers : {
                Authorization : 'Bearer ' + sessionStorage.getItem('token')
             }
            }).then(response => {
               this.setState({contact : response.data}) 
               console.log('userContact' , response)
               response.data.map((contact) => {
                    List.push(<option value = {contact.id}>{contact.name}</option>)
               })

               this.setState({contactOptions : List})
             })



        }



        date = (e) =>
        {
            let fullDate = e.value._d
            let date  =fullDate.toISOString()
            this.setState({date : date})
            let formData = this.state.factorData
            formData.date = date
            this.setState({factorData : formData})


        }
        time = (e) =>
        {
            console.log(e.target.value)
            
        }

        selectContact = (e) => {
            this.setState({selectContact : e.target.value})
            let formData = this.state.factorData
            let contanctName = e.target.value
            ///


            formData.contactId = e.target.value
            console.log(e.target.value)
            this.setState({factorData : formData})
            
        }



        addItem = () => {
            let prev = this.state.factorItem
            prev = prev.concat(Date.now())
            this.setState({factorItem : prev })
            this.setState({endOfFactor : true})
        }
   


        endOfFactor = (e)=>{
            this.setState({endOfFactorClicked : true})
            console.log(this.state.factorData)
            axios.post('http://app.bazarsefid.com/api/Administrator/SubmitUserFactor' , this.state.factorData , { headers: {
                Authorization : 'Bearer ' + sessionStorage.getItem('token')
            }}).then(response => {
                console.log(response)
                this.setState({endOfFactorClicked: false})
            })
            
        }




        selectUsers = (e) => {
            
            this.setState({selectedUserPhone : e.target.value})
        }



     render() {
      
        
       
       


        return (
           
               
        <div>
                <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.13.0/css/all.css" integrity="sha384-Bfad6CLCknfcloXFOyFnlgtENryhrpZCe29RTifKEixXQZ38WheV+i/6YWSzkz3V" crossorigin="anonymous" type="text/css"></link>

                <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" integrity="sha384-9aIt2nRpC12Uk9gS9baDl411NQApFmC26EwAOH8WgZl5MYYxFfc+NcPb1dKGj7Sk" crossorigin="anonymous"></link>
                <div className = 'row'>
                    <select name = "select" id = "contacts"  onChange = {this.selectUsers} className = "form-control">
                        {this.state.usersOption}
                    </select>
                </div>
                <div className = "row">
                    <div className = "col-5">
                        <button className= "btn btn-primary btn-block" id ="box" onClick={this.Box} disabled = {this.state.makeFactor}>Make Factor</button>
                         {this.state.makeFactor && <button  className= "btn btn-primary "  onClick = {this.addItem}>+</button>}
                         <Fields Items = {this.state.factorItem} setItem ={this.setItem}></Fields>
                         {this.state.endOfFactor && <select name = "select" id ='items' className = "form-control" onChange = {this.selectContact} >
                                    {this.state.contactOptions}
                                </select>}
                        {this.state.endOfFactor && <DatePicker  onClickSubmitButton = {this.date.bind(this)}></DatePicker> }
                         {this.state.endOfFactor && <button className= "btn btn-primary btn-block" onClick = {this.endOfFactor} disabled = {this.state.endOfFactorClicked} >End Of Factor</button>}
                         
                         
                         
                    </div>
                    
                    <div className = "col-5">
                            <ImageGrid images = {this.state.images}  ></ImageGrid>
                    </div>

                    <div className = "col-2">
                    <button type="button" className="btn btn-primary btn-block" id = "fetch" onClick = {this.fetchFactor} disabled = {this.state.loading}>
                        {this.state.loading && (<i className ="fa fa-refresh fa-spin"></i>)}
                            Fetch Undone Factors
                        </button>
                        
                        <FactorGrid factors = {this.state.factors}  showFactor = {this.showFactor}></FactorGrid>
                    </div>
                </div>
                
        </div>
            
        )
    }
}

export default Dashboard
