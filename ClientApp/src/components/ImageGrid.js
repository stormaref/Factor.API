import React, { Component } from 'react'

export class ImageGrid extends Component {
    render() {
        
        let src = "data:image/jpeg;base64," + this.props.images
        return this.props.images.map((image)=>(
            <img  src = {image} className = "img-fluid w-100" ></img>
        ))
    }
}

export default ImageGrid
