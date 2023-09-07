import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useWebsiteTesterStore } from '../stores/WebsiteTesterStore';

function TestDetailsView() {
  const { id } = useParams();
  const store = useWebsiteTesterStore();
  const [testDetails, setTestDetails] = useState(null);

  useEffect(() => {
    fetchDetails();
  }, [id, store]);

  const fetchDetails = () => {
    store.fetchTestDetails(id).then((data) => {
      setTestDetails(data);
    });
  };

  const renderSortedResults = (filterFn) => {
    const filteredResults = testDetails?.testResults?.slice().filter(filterFn);
    const sortedResults = filteredResults?.sort((a, b) => a.renderTimeMilliseconds - b.renderTimeMilliseconds);

    return (
      <table className="table">
        <thead>
          <tr>
            <th>Url</th>
            <th>Timing</th>
          </tr>
        </thead>
        <tbody>
          {sortedResults?.map((result, index) => (
            <tr key={index}>
              <td>{result.url}</td>
              <td>{result.renderTimeMilliseconds}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  };

  return (
    <div>
      <h2 className="mt-5">{testDetails?.url}</h2>

      <div className="mt-5">
        <h2>PERFORMANCE</h2>
        {renderSortedResults(() => true)}
      </div>

      <div className="mt-5">
        <h2>URLs NOT FOUND AT WEBSITE</h2>
        {renderSortedResults((x) => x.isInSitemap && !x.isInWebsite)}
      </div>

      <div className="mt-5">
        <h2>URLs NOT FOUND AT SITEMAP</h2>
        {renderSortedResults((x) => x.isInWebsite && !x.isInSitemap)}
      </div>
    </div>
  );
}

export default TestDetailsView;