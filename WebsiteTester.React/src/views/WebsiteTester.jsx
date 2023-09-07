import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useWebsiteTesterStore } from '../stores/WebsiteTesterStore';

function WebsiteTesterComponent() {
  const store = useWebsiteTesterStore();
  const [formUrl, setFormUrl] = useState('');
  const [isTesting, setIsTesting] = useState(false);

  useEffect(() => {
    fetchLinks();
  }, [store]);

  const links = store.$state.links;
  const url = store.$state.testingUrl || '';

  const fetchLinks = () => {
    store.fetchLinks();
  };

  const testUrl = () => {
    setIsTesting(true);
    store.testUrl(formUrl).then((data) => {
      if (data) {
        setFormUrl('');
        setIsTesting(false);
        fetchLinks();
      }
    });
  };

  return (
    <div>
      <div className="d-flex flex-row gap-3 mt-0">
        <div className="form-group d-flex align-items-center flex-row w-100">
          <label className="w-25">Enter a website</label>
          <input
            value={formUrl}
            onChange={(e) => setFormUrl(e.target.value)}
            type="text"
            className="form-control"
            id="url"
            name="url"
            placeholder="Enter URL"
          />
        </div>
        <div className="p-1">
          <button
            type="submit"
            className="btn btn-primary"
            onClick={testUrl}
            id="testUrlButton"
            disabled={isTesting}
          >
            TEST
            {isTesting && (
              <span
                className="spinner-border spinner-border-sm"
                role="status"
                aria-hidden="true"
              ></span>
            )}
          </button>
        </div>
      </div>
      <div>
        <h2 className="mt-5">Test Results</h2>
        <table className="table mt-5">
          <thead>
            <tr>
              <th scope="col">Website</th>
              <th scope="col">Date</th>
              <th scope="col"></th>
            </tr>
          </thead>
          <tbody>
            {links.map((link) => (
              <tr key={link.id}>
                <td>{link.url}</td>
                <td>
                  {new Date(link.createdOn).toLocaleTimeString('default', {
                    hour: '2-digit',
                    minute: '2-digit',
                  }) +
                    ' ' +
                    new Date(link.createdOn).toLocaleDateString('default', {
                      day: '2-digit',
                      month: '2-digit',
                      year: 'numeric',
                    })}
                </td>
                <td>
                  <Link
                    to={{
                      pathname: `/details/${link.id}`,
                    }}
                  >
                    see details
                  </Link>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default WebsiteTesterComponent;