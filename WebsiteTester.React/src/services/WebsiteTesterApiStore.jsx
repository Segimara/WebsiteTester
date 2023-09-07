import axios from 'axios';

export class WebsiteTesterApiClient {
  constructor(baseURL) {
    this.http = axios.create({
      baseURL,
    });
  }

  async getLinks() {
    try {
      const response = await this.http.get('/tests');
      return response.data;
    } catch (error) {
      console.error('Error fetching links:', error);
      throw error;
    }
  }

  async getLink(id) {
    try {
      const response = await this.http.get(`/tests/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching link with ID ${id}:`, error);
      throw error;
    }
  }

  async testUrl(url) {
    try {
      const response = await this.http.post('/tests/test/', { url });
      return response.data;
    } catch (error) {
      console.error('Error testing URL:', error);
      throw error;
    }
  }
}
