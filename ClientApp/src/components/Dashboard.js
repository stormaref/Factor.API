import React, {Component} from 'react'


import axios from 'axios'

import FactorGrid from './FactorGrid'

import ImageGrid from './ImageGrid'

import Fields from './Fields'


import {DatePicker} from "jalali-react-datepicker";

import Combo from './Combo'
import {wait} from '@testing-library/react'

export class Dashboard extends Component {

    state = {
        contactAddCheck: false,
        contactMessage: '',
        itemAddCheck: false,
        itemMessage: '',
        checkMessage: false,
        messagetext: "",
        forceItemUpdate: false,
        addNewContact: false,
        addNewItem: false,
        newContactPhone: '',
        newProductName: '',
        newContactName: '',
        users: [],
        selectedUserPhone: '',
        usersOption: [],
        images: [],
        factors: [],
        loading: false,
        items: [],
        makeFactor: false,
        factorItem: [],
        toBeSent: [],
        endOfFactor: false,
        selectedPreFactor: [],
        contacts: [],
        contactOptions: [],
        date: '',
        selectedContact: '',
        factorData: {

            factorItems: [],
            preFactorId: '',
            state: {
                isClear: true
            },
            contactId: '',
            date: ''
        },
        endOfFactorClicked: false

    }

    onSubmit = async (e) => {
        e.preventDefault()


        // response.data.map((image) => (
        //     this.state.images.push(image)
        // ))

        // console.log(this.state.images)


    }


    componentDidMount() {

        let data = this.state.users

        let options = this.state.usersOption
        options.push (
            <option></option>
        )
        axios.get('http://app.bazarsefid.com/api/Administrator/GetAllUsers', {
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(response => {
            console.log(response.data)
            response.data.map((user) => (options.push (
                <option value={
                    user.phone
                }>
                    {
                    user.phone
                }</option>
            )))
            this.setState({usersOption: options})

            response.data.map((user) => (data.push(user)))
            console.log(data)
            this.setState({users: data})


        })
    }


    showFactor = (data) => {
        const factors = this.state.factors
        let no = 0

        for (let index = 0; index < factors.length; index++) {
            if (factors[index].id == data) {
                no = index
                break;
            }

        }


        this.setState({selectedPreFactor: factors[no].id})


        this.setState({images: factors[no].images})

    }


    setItem = (no, value, total, item, data) => {
        console.log(no)
        console.log(value)
        console.log(no * value)
        console.log(item)
        console.log(data)
        console.log(this.state.selectedPreFactor)
        console.log(this.state.date)
        console.log(this.state.selectedContact)
        let Data = this.state.factorData
        Data.preFactorId = this.state.selectedPreFactor

        Data.factorItems.push({
            product: {
                title: item

            },
            quantity: Number(no),
            price: Number(value)
        })

        this.setState({factorData: Data})


    }


    Box = async () => {
        this.setState({makeFactor: true})
        let prev = this.state.factorItem
        prev = prev.concat(Date.now())
        this.setState({factorItem: prev})
        this.setState({endOfFactor: true})


        let List = [
            <option></option>
        ]

        await axios.get('http://app.bazarsefid.com/api/Administrator/GetUserContacts', {
            params: {
                phone: this.state.selectedUserPhone
            },
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(response => {
            this.setState({contact: response.data})
            console.log('userContact', response)
            response.data.map((contact) => {
                List.push (
                    <option value={
                        contact.id
                    }>
                        {
                        contact.name
                    }</option>
                )
            })

            this.setState({contactOptions: List})
        })


    }


    date = (e) => {
        let fullDate = e.value._d
        let date = fullDate.toISOString()
        this.setState({date: date})
        let formData = this.state.factorData
        formData.date = date
        this.setState({factorData: formData})


    }
    time = (e) => {
        console.log(e.target.value)

    }

    selectContact = (e) => {
        this.setState({selectContact: e.target.value})
        let formData = this.state.factorData
        let contanctName = e.target.value
        // /


        formData.contactId = e.target.value
        console.log(e.target.value)
        this.setState({factorData: formData})

    }


    addItem = () => {
        let prev = this.state.factorItem
        prev = prev.concat(Date.now())
        this.setState({factorItem: prev})
        this.setState({endOfFactor: true})
    }


    endOfFactor = async (e) => {
        this.setState({endOfFactorClicked: true})
        console.log(this.state.factorData)
        await axios.post('http://app.bazarsefid.com/api/Administrator/SubmitUserFactor', this.state.factorData, {
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(response => {
            console.log('this is the response from submiting', response)
            this.setState({endOfFactorClicked: false})
            this.setState({endOfFactor: true})
            if (response.status == 200) {
                this.setState({checkMessage: true})
                this.setState({messagetext: 'factor submited !!!'})
            }
        })

    }


    selectUsers = (e) => {


        this.setState({factors: []})
        console.log(e.target.value)
        this.setState({loading: true})
        const response = axios.get('http://app.bazarsefid.com/api/Administrator/GetUserUndoneFactors', {
            params: {
                phone: e.target.value
            },
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(response => {
            console.log(response.data)


            this.setState({factors: response.data})


            this.setState({loading: false})


        })

        this.setState({selectedUserPhone: e.target.value})

    }


    submitNewProductHandler = () => {
        this.setState({addNewItem: true})

    }


    setNewProductName = (e) => {
        this.setState({newProductName: e.target.value})
    }


    submitNewItem = (e) => {
        axios.post('http://app.bazarsefid.com/api/Administrator/AddProduct', null, {
            params: {
                title: this.state.newProductName
            },
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(response => {
            if (response.status == 200) {
                this.setState({itemAddCheck: true})
                this.setState({itemMessage: 'item Added ....'})
                let factoritem = this.state.factorItem
                this.setState({factorItem: factoritem})
                this.setState({addNewItem: false})
                this.setState({forceItemUpdate: true})
            }
        })
    }


    submitNewContactControler = () => {
        this.setState({addNewContact: true})
    }


    submitNewContact = (e) => {

        let user = this.state.selectedUserPhone
        console.log(user)
        axios.post('http://app.bazarsefid.com/api/Administrator/AddContactToUser', {
            UserPhone: user,
            contactName: this.state.newContactName
        }, {
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(response => {
            console.log(response)
            if (response.status == 200) {
                this.setState({contactAddCheck: true})
                this.setState({contactMessage: 'User Added ...'})
                let contactOptions = this.state.contactOptions
                contactOptions.push (
                    <option value={
                        response.data.id
                    }>
                        {
                        response.data.name
                    }</option>
                )
                this.setState({contactOptions: contactOptions})
                this.setState({addNewContact: false})


            }
        })


    }


    setNewContactName = (e) => {
        this.setState({newContactName: e.target.value})
    }


    backgroundColor = {
        backgroundColor: '#8A2BE2'
    }


    closeAlert = () => {
        this.setState({checkMessage: false})
        this.setState({itemAddCheck: false})
        this.setState({contactAddCheck: false})
    }

    render() {


        return (


            <div style={
                {
                    overflow: "hidden",
                    height: "100%"
                }
            }>
                {
                this.state.checkMessage && <div class="alert alert-success alert-dismissible fade show" role="alert">
                    <strong>Warning!</strong>
                    {
                    this.state.messagetext
                }
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close" onClick ={this.closeAlert}>
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            }

                {
                this.state.itemAddCheck && <div class="alert alert-success alert-dismissible fade show" role="alert">
                    <strong>Success!</strong>
                    {
                    this.state.itemMessage
                }
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close" onClick ={this.closeAlert}>
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            }

                {
                this.state.contactAddCheck && <div class="alert alert-success alert-dismissible fade show" role="alert">
                    <strong>Success!</strong>
                    {
                    this.state.contactMessage
                }
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close" onClick ={this.closeAlert}>
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            }
                <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.13.0/css/all.css" integrity="sha384-Bfad6CLCknfcloXFOyFnlgtENryhrpZCe29RTifKEixXQZ38WheV+i/6YWSzkz3V" crossorigin="anonymous" type="text/css"></link>
                <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" integrity="sha384-9aIt2nRpC12Uk9gS9baDl411NQApFmC26EwAOH8WgZl5MYYxFfc+NcPb1dKGj7Sk" crossorigin="anonymous"></link>

                <nav className="navbar navbar-light"
                    style={
                        {
                            backgroundColor: "#660099",
                            height: "60px"
                        }
                }>
                    <div style={
                        {width: "33%"}
                    }>
                        <label style={
                            {
                                color: "#fff",
                                float: "left",
                                textAlign: "center"
                            }
                        }>Select Users</label>
                        <select className="form-control"
                            style={
                                {
                                    float: "left",
                                    width: "60%",
                                    marginLeft: "20px"
                                }
                            }
                            onChange={
                                this.selectUsers
                        }>
                            {
                            this.state.usersOption
                        }</select>
                    </div>

                    <div className="form-inline">
                        <button className="btn btn-outline-warning">All factors</button>
                    </div>
                </nav>

                <div className="row"
                    style={
                        {paddingLeft: "10px"}
                }>
                    <div class="col-3 order-2" id="sticky-sidebar"
                        style={
                            {
                                borderLeft: "1px solid #660099",
                                paddingRight: "30px"
                            }
                    }>
                        <ul class="sticky-top list-group"
                            style={
                                {paddingTop: "10px"}
                        }>
                            <FactorGrid factors={
                                    this.state.factors
                                }
                                showFactor={
                                    this.showFactor
                                }
                                className='form-control'></FactorGrid>
                        </ul>
                    </div>

                    <div className="col-9">
                        <div className="row"
                            style={
                                {paddingTop: "10px"}
                        }>
                            <div className="col-5">
                                <button className="btn btn-primary btn-block" id="box"
                                    onClick={
                                        this.Box
                                    }
                                    disabled={
                                        this.state.makeFactor
                                    }
                                    style={
                                        this.backgroundColor
                                }>Make Factor</button>
                                {
                                this.state.makeFactor && <button className="btn btn-primary "
                                    onClick={
                                        this.addItem
                                    }
                                    style={
                                        this.backgroundColor
                                }>+</button>
                            }
                                <Fields Items={
                                        this.state.factorItem
                                    }
                                    setItem
                                    ={this.setItem}
                                    bool={
                                        this.state.forceItemUpdate
                                }></Fields>
                                {
                                this.state.endOfFactor && <select name="select" id='items' className="form-control"
                                    onChange={
                                        this.selectContact
                                }>
                                    {
                                    this.state.contactOptions
                                } </select>
                            }

                                {
                                this.state.endOfFactor && <button className='btn btn-primary form-control'
                                    onClick={
                                        this.submitNewContactControler
                                    }
                                    disabled
                                    ={this.state.addNewContact}>add new Contact</button>
                            }
                                {
                                this.state.addNewContact && <input type='text' className='form-control' placeholder='new Contact Name' onChange ={this.setNewContactName}></input>
                            }
                                {
                                this.state.addNewContact && <button className='btn btn-primary form-control' onClick ={this.submitNewContact}>Submit New Contact</button>
                            }


                                {
                                this.state.endOfFactor && <button className='btn btn-primary form-control'
                                    onClick={
                                        this.submitNewProductHandler
                                    }
                                    disabled
                                    ={this.state.addNewContact}>add new Product</button>
                            }
                                {
                                this.state.addNewItem && <input type='text' className='form-control' placeholder='Item Name' onChange ={this.setNewProductName}></input>
                            }
                                {
                                this.state.addNewItem && <button className='btn btn-primary form-control' onClick ={this.submitNewItem}>Submit New Product</button>
                            }


                                {
                                this.state.endOfFactor && <DatePicker onClickSubmitButton={
                                    this.date.bind(this)
                                }></DatePicker>
                            }
                                {
                                this.state.endOfFactor && <button className="btn btn-primary btn-block"
                                    onClick={
                                        this.endOfFactor
                                    }
                                    disabled={
                                        this.state.endOfFactorClicked
                                    }
                                    style={
                                        this.backgroundColor
                                }>End Of Factor</button>
                            } </div>

                            <div className="col-7">
                                <ImageGrid images={
                                    this.state.images
                                }></ImageGrid>
                            </div>


                        </div>
                    </div>
                </div>

            </div>

        )
    }
}

export default Dashboard
