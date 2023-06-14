import type { Link } from "@/models/Link";
import axios, { type AxiosInstance } from "axios";

export class WebsiteTesterApiClient {
  private http: AxiosInstance;

  constructor(baseURL: string) {
    this.http = axios.create({
      baseURL,
    });
  }

  async getLinks(): Promise<Link[]> {
    const response = await this.http.get("/tests");
    return response.data;
  }

  async getLink(id: string): Promise<Link> {
    const response = await this.http.get(`/tests/${id}`);
    return response.data;
  }

  async testUrl(url: string): Promise<boolean> {
    const response = await this.http.post(`/tests/test/`, { url });
    return response.data;
  }
}