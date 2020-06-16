import React from 'react'

import {Route , Redirect} from 'react-router-dom'

const  PrivateRoute = ({component : Component   , ...rest}) => (
  <Route
    {...rest}
    render={props => {
       if(!sessionStorage.getItem('Granted'))
       {
           
           return <Redirect to = "/" />
           
       }
       else if(sessionStorage.getItem('Granted'))
       {
          
            return <Component  />
       }
    }}
  
  
  />

  
)


export default PrivateRoute ; 