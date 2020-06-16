import React, { Component } from 'react'

export class ImageGrid extends Component {
    render() {
        console.log(this.props.images)
        let src = "data:image/jpeg;base64," + this.props.images
        return this.props.images.map((image)=>(
            <img  src = { "data:image/jpeg;base64," + image.bytes} className = "img-fluid" ></img>
        ))
    }
}

export default ImageGrid
