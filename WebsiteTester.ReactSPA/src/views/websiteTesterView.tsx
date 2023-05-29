
import React, { Component } from 'react';
import { Link, WebsiteTesterApiClient } from '../services/WebsiteTesterApiClient';

class WebsiteTesterView extends Component {
  websiteTesterApiClient: any;
  constructor(props: any) {
    super(props);

    this.state = {
      links: [] as Link[],
      formUrl: "qwet 123"
    };

    this.websiteTesterApiClient = new WebsiteTesterApiClient(import.meta.env.VITE_API_URL);
  }

  render() {
    return (
      <>
        <div className="d-flex flex-row gap-3 mt-0">
          <div className="form-group d-flex align-items-center flex-row w-100">
            <label className="w-25">Enter a website</label>
            <input
              value={this.state.formUrl}
              type="text"
              className="form-control"
              id="url"
              name="url"
              placeholder="Enter URL"
              onChange={this.handleFormUrlChange}
            />
          </div>
          <div className="p-1">
            <button
              type="submit"
              className="btn btn-primary"
              onClick={() => this.testUrl()}
              id="testUrlButton"
              data-loading-text="Loading..."
            >
              TEST
            </button>
          </div>
        </div>
        <div className="">
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
              {this.state.links.map((link: Link) => (
                <tr>
                  <td>{link.url}</td>
                  <td>{new Date(link.createdOn).toLocaleTimeString('default', { hour: '2-digit', minute: '2-digit' }) +
                    " " +
                    new Date(link.createdOn).toLocaleDateString('default', {
                      day: '2-digit', month: '2-digit', year:
                        'numeric'
                    })
                  }</td>
                  <td>
                    <RouterLink >see details</RouterLink>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </>
    );
  }
  handleFormUrlChange = (event: any) => {
    this.setState({
      formUrl: event.target.value
    })
  }
  async fetchLinks() {
    const links = await this.websiteTesterApiClient.getLinks();
    this.setState({ links });
  }
  async testUrl() {
    const result = await this.websiteTesterApiClient.testUrl(this.state.formUrl);
    if (result) {
      this.setState({ formUrl: "" });
      await this.fetchLinks();
    }
  }
}