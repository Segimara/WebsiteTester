import React, { createContext, useContext } from 'react';
import { WebsiteTesterApiClient } from '../Services/WebsiteTesterApiStore.jsx';

// Create a context for your store
const WebsiteTesterContext = createContext();

export const useWebsiteTesterStore = () => {
  return useContext(WebsiteTesterContext);
};

export const WebsiteTesterProvider = ({ children }) => {
  const WebsiteTesterApiBaseUrl = import.meta.env.VITE_APP_BASEURL;

  state = {
    links: [],
    testingUrl: null,
    isUrlBeingTested: false,
  };

  const fetchLinks = async () => {
    const client = new WebsiteTesterApiClient(WebsiteTesterApiBaseUrl);
    const links = await client.getLinks();
    setState({ ...state, links });
  };

  const fetchTestDetails = async (link) => {
    const client = new WebsiteTesterApiClient(WebsiteTesterApiBaseUrl);
    return await client.getLink(link);
  };

  const testUrl = async (url) => {
    const client = new WebsiteTesterApiClient(WebsiteTesterApiBaseUrl);
    setState({ ...state, testingUrl: url, isUrlBeingTested: true });
    const result = await client.testUrl(url);
    setState({ ...state, testingUrl: null, isUrlBeingTested: false });
    return result;
  };

  const setState = (newState) => {
    state = { ...state, ...newState };
  };

  return (
    <WebsiteTesterContext.Provider
      value={{
        ...state,
        fetchLinks,
        fetchTestDetails,
        testUrl,
      }}
    >
      {children}
    </WebsiteTesterContext.Provider>
  );
};
