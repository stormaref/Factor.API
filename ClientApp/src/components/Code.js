import React, { Component } from 'react'
import axios from 'axios'




export class Code extends Component {

    state = {
        code : '' ,
        message : ''
    }



    onChangeU = (e) => {
        this.setState({code : e.target.value})
        
    }

    onSubmit = async (e) => {
        e.preventDefault()
        await axios.post('http://app.bazarsefid.com/api/Login/VerifyCode' ,{ 
        phone : sessionStorage.getItem('phone') ,
        code : Number(this.state.code)
         }).then(response => {
        if (response.status === 400 )
        {
            console.log("wtf")
            document.getElementById('message').hidden = false
        }
        
        else if( response.status ===200 )
        {
            console.log(response)
            sessionStorage.setItem('token' , response.data.token)
            sessionStorage.setItem('Granted' , true)
            window.location.href = '/dashboard'
        }
       }).catch(err => {
          console.log(err.response)
       })
        
        
    }


    render() {
        return (
            <div>
               <link href="https://stackpath.bootstrapcdn.com/bootswatch/4.5.0/pulse/bootstrap.min.css" rel="stylesheet" integrity="sha384-t87SWLASAVDfD3SOypT7WDQZv9X6r0mq1lMEc6m1/+tAVfCXosegm1BvaIiQm3zB" crossorigin="anonymous"></link>
                <div className="row mt-5">
            <div className="col-md-6 m-auto">
            <div className="card card-body">
                <h1 classNmae="text-center mb-3"><i className="fas fa-sign-in-alt"></i>Code Verify</h1>
                <form id="form" onSubmit = {this.onSubmit}  >
                <div className="form-group">
                    <label for="code">Code</label>
                    <input
                    onChange = {this.onChangeU}
                    type="text"
                    id="code"
                    name="code"
                    value= {this.state.code}
                    className="form-control"
                    placeholder="Enter Code"
                    />
                </div>
                <button type="btn" className="btn btn-primary btn-block" id="btn">Send Activation Code</button>
                
                </form>
                <br/>
                <form id="form" onSubmit = {this.onSubmit}  >
                <br/>
                
                <button type="btn" className="btn btn-primary btn-block" id="btn">Resend Code</button>
                
                </form>

            </div>
            </div>
      </div>
            </div> 
            
        )
    }
}

export default Code
