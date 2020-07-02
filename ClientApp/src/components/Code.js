import React, { Component } from 'react'
import axios from 'axios'




export class Code extends Component {

    state = {
        code : '' ,
        message : '',
        state : false
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

            this.setState({state : true})
            console.log("wtf")
            
        }
        
        else if( response.status ===200 )
        {
            console.log(response)
            sessionStorage.setItem('token' , response.data.token)
            sessionStorage.setItem('Granted' , true)
            window.location.href = 'dashboard'
        }
       }).catch(err => {
        if(err.response.data == 'Code is incorrect')
        {
            this.setState({state : true})
            this.setState({message: err.response.data})
        }
        else {
            this.setState({state : true})
            this.setState({message: err.response.data.title})
              console.log(err.response)
        }


       
       })
        
        
    }

    closeAlert = () => {
        this.setState({state : false})
    }


    render() {
        return (
            <div>
                
               <link href="https://stackpath.bootstrapcdn.com/bootswatch/4.5.0/pulse/bootstrap.min.css" rel="stylesheet" integrity="sha384-t87SWLASAVDfD3SOypT7WDQZv9X6r0mq1lMEc6m1/+tAVfCXosegm1BvaIiQm3zB" crossorigin="anonymous"></link>
                <div className="row mt-5">
                    <div className="col-md-6 m-auto">
                        
                            <h1 classNmae="text-center mb-3"><i className="fas fa-sign-in-alt"></i>Code Verify</h1>
                            <div className="card card-body">
                        {this.state.state && <div class="alert alert-warning alert-dismissible fade show" role="alert">
                            <strong>Warning!</strong> {this.state.message}
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close" onClick ={this.closeAlert}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>}
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
             <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js" integrity="sha384-OgVRvuATP1z7JjHLkuOU7Xw704+h835Lr+6QL9UvYjZE3Ipu6Tp75j7Bh/kR0JKI" crossorigin="anonymous"></script>
        </div>
            
        )
    }
}

export default Code
