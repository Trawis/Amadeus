import React from 'react';
import { Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHome, faHotel } from '@fortawesome/free-solid-svg-icons'

export function NavMenu() {
    return (
        <Navbar collapseOnSelect>
            <Navbar.Brand>
                <LinkContainer to='/'>
                    <NavItem>AHS</NavItem>
                </LinkContainer>
            </Navbar.Brand>
            <Navbar.Toggle />
            <Navbar.Collapse>
                <Nav justify>
                    <LinkContainer to='/' exact>
                        <NavItem>
                            <FontAwesomeIcon icon={faHome} /> Home
                        </NavItem>
                    </LinkContainer>
                    <LinkContainer to='/hotels'>
                        <NavItem>
                            <FontAwesomeIcon icon={faHotel} /> Search Hotels
                        </NavItem>
                    </LinkContainer>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
}