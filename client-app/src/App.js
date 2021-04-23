import React from 'react';
import { Route } from 'react-router';
import { BrowserRouter as Router } from 'react-router-dom';
import { Home } from './home';
import { Hotels } from './hotels';
import { Layout } from './navigation';

function App() {
  return (
    <Router>
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/hotels' component={Hotels} />
      </Layout>
    </Router>
  );
}

export default App;