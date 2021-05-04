import React, { useState } from 'react';
import { Col, Row, Button, Table, Alert } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { DatetimePickerTrigger } from 'imrc-datetime-picker';
import moment from 'moment-jalaali';
import { API } from '../../core';
import { Textbox } from '../../shared';
import "imrc-datetime-picker/dist/imrc-datetime-picker.css";

export function Hotels() {
    const [momentValue, setMomentValue] = useState(moment());
    const [errors, setErrors] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [hotels, setHotels] = useState([]);
    const [cityCode, setCityCode] = useState('');
    const [checkInDate, setCheckInDate] = useState('');
    const [checkOutDate, setCheckOutDate] = useState('');

    const search = async () => {
        if (cityCode === '' || checkInDate === '' || checkOutDate === '') {
            return setErrors(["Search parameters cannot be empty."]);
        }

        let hotels = [];

        setErrors([]);
        setIsLoading(true);
        setHotels(hotels);
        try {
            const request = {
                "cityCode": cityCode,
                "checkInDate": checkInDate,
                "checkOutDate": checkOutDate
            };

            const response = await API.hotel.search(request);

            if (!response) {
                return setErrors(["An error has occurred on searching hotels."]);
            }
            
            hotels = response.result;
        } catch (e) {
            if (e.errors) {
                setErrors(e.errors.map(error => error.message));
            } else {
                setErrors([e.message]);
            }
        } finally {
            setIsLoading(false);
            setHotels(hotels);
        }
    };

    const handleCheckInDateChange = (moment) => {
        setCheckInDate(moment.format("YYYY-MM-DD"));
        setMomentValue(moment);
    };

    const handleCheckOutDateChange = (moment) => {
        setCheckOutDate(moment.format("YYYY-MM-DD"));
        setMomentValue(moment);
    };

    return (
        <Col>
            <h1>Hotels</h1>
            <p>Search Hotels by parameters</p>
            <Row>
                <Col md={4}>
                    <Textbox
                        id="cityCodeId"
                        name="cityCode"
                        label="Write city code"
                        value={cityCode}
                        onChange={e => setCityCode(e.target.value)}
                    />
                </Col>
                <Col md={4}>
                    <DatetimePickerTrigger
                        moment={momentValue}
                        onChange={(_moment) => handleCheckInDateChange(_moment)}
                        showTimePicker={false}
                        minPanel="day">
                        <Textbox
                            id="checkInDateId"
                            name="checkInDate"
                            label="Choose check-in date"
                            value={checkInDate}
                            readOnly={true}
                        />
                    </DatetimePickerTrigger>
                </Col>
                <Col md={4}>
                    <DatetimePickerTrigger
                        moment={momentValue}
                        onChange={(_moment) => handleCheckOutDateChange(_moment)}
                        showTimePicker={false}
                        minPanel="day">
                        <Textbox
                            id="checkOutDateId"
                            name="checkOutDate"
                            label="Choose check-out date"
                            value={checkOutDate}
                            readOnly={true}
                        />
                    </DatetimePickerTrigger>
                </Col>
            </Row>
            <Row>
                <Col md={3}>
                    <p><Button variant="primary" onClick={search}><FontAwesomeIcon icon={faSearch} /> Search</Button></p>
                </Col>
            </Row>
            {errors && errors.length > 0 &&
                <Alert variant="danger">
                    <ul>
                        {errors.map((error, i) =>
                            <li key={i}>{error}</li>
                        )}
                    </ul>
                </Alert>
            }
            <Table striped bordered responsive size="sm">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Rating</th>
                        <th>Available</th>
                        <th>Currency</th>
                        <th>Total</th>
                    </tr>
                </thead>
                <tbody>
                    {isLoading && "Loading..."}
                    {hotels && hotels.map((hotel) =>
                        <tr key={hotel.hotelId}>
                            <td>{hotel.name}</td>
                            <td>{hotel.description}</td>
                            <td>{hotel.rating}</td>
                            <td>{hotel.avaliable}</td>
                            <td>{hotel.currency}</td>
                            <td>{hotel.total}</td>
                        </tr>
                    )}
                </tbody>
            </Table>
        </Col>
    );
};