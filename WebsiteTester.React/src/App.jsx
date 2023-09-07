import { BrowserRouter as Router, Route, Link, Switch } from 'react-router-dom';
import WebsiteTester from '../views/WebsiteTesterView';
import TestDetailsViewReact from '../views/TestDetailsViewReact';
import { WebsiteTesterProvider } from './contexts/websiteTesterContext.jsx'; // Import your provider

function Header() {
  return (
    <header>
      <div className="wrapper">
        <nav>
          {/* Add your navigation links here using React Router's Link component */}
          <Link to="/">Home</Link>
          <Link to="/about">About</Link>
          {/* Add more links as needed */}
        </nav>
      </div>
    </header>
  );
}
function App() {
  return (
    <Router>
      <WebsiteTesterProvider>
        <Switch>
          <Route path="/" exact component={WebsiteTester} />
          <Route path="/details/:id" component={TestDetailsViewReact} />
        </Switch>
      </WebsiteTesterProvider>
    </Router>
  );
}

export default App;