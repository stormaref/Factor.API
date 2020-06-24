import React, { Component } from 'react'

import axios from 'axios'
import { Redirect, Link, Router } from 'react-router-dom'



export class Login extends Component {

    state = {
        Username : '',
        Granted : false 
    }
    



    onChangeU = (e) => {
        this.setState({Username : e.target.value})
       
    }
     onSubmit = async (e) => 
    {
        
        e.preventDefault()
        const response  = await axios.post('http://app.bazarsefid.com/api/Administrator/AdminLogin' , null , { params : {
            phone : this.state.Username 
        }}).then(response => {
            console.log(response)
        if(response.status == 200)
        {
            
            sessionStorage.setItem('phone' , this.state.Username )
            console.log('accepted')
            window.location.href = '/Code'
        }
        if(response.status ==401)
        {
            this.setState({Granted : false})
        }
        console.log(response)
        }).catch(err =>
            {
                if(err.response.statusText == 'Unauthorized')
                {
                    console.log('phone is not valid')
                }
                else {
                    console.log('phone format is wrong')
                }
            })
            
        
        

        
        
        this.setState({
            Username : '' 
        })


        this.props.loginGranted(this.state.Granted)
        console.log(this.state.Granted)





      
        
    }
    render() {
        return (
            <div>
                
                <link href="https://stackpath.bootstrapcdn.com/bootswatch/4.5.0/solar/bootstrap.min.css" rel="stylesheet" integrity="sha384-iDw+DjLp94cdk+ODAgTY4IZ6d9aaRpG9KHr168TPxrfQ9wv/DTVC+cWyojoxjHBT" crossorigin="anonymous"></link>
                <div class="row mt-5">
        <div class="col-md-6 m-auto">
          <div class="card card-body">
            <h1 class="text-center mb-3"><i class="fas fa-sign-in-alt"></i>  Login</h1>
            <form id="form" onSubmit = {this.onSubmit}  >
              <div class="form-group">
                <label for="email">Phone</label>
                <input
                  onChange = {this.onChangeU}
                  type="text"
                  id="username"
                  name="email"
                  value= {this.state.Username}
                  class="form-control"
                  placeholder="Enter PhoneNumber"
                />
              </div> 
             <button type="btn" class="btn btn-primary btn-block" id="btn">Login</button>
            </form>
            
            <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js" integrity="sha384-OgVRvuATP1z7JjHLkuOU7Xw704+h835Lr+6QL9UvYjZE3Ipu6Tp75j7Bh/kR0JKI" crossorigin="anonymous"></script>
          </div>
        </div>
      </div>
      

                
    </div> 
           
        )
    }
}

export default Login
