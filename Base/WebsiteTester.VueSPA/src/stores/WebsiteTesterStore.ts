import type { Link } from "@/models/Link";
import { WebsiteTesterApiClient } from "@/services/WebsiteTesterApiClient";
import { defineStore } from "pinia";
import { ref, type Ref } from "vue";

const WebsiteTesterApiBaseUrl = import.meta.env.VITE_APP_BASEURL;

export const useWebsiteTesterStore = defineStore({
  id: "websiteTester",
  state: () => ({
    links: ref([]) as Ref<Link[]>,
    testingUrl: ref(null) as Ref<string | null>,
    isUrlBeingTested: ref(false),
  }),
  actions: {
    async fetchLinks() {
      const client = new WebsiteTesterApiClient(WebsiteTesterApiBaseUrl);
      const links = await client.getLinks();
      this.links = links;
    },

    async fetchTestDetails(link: string): Promise<Link> {
      const client = new WebsiteTesterApiClient(WebsiteTesterApiBaseUrl);
      return await client.getLink(link);
    },

    async testUrl(url: string) {
      const client = new WebsiteTesterApiClient(WebsiteTesterApiBaseUrl);
      this.testingUrl = url;
      this.isUrlBeingTested = true;
      const result = await client.testUrl(url);
      this.testingUrl = null;
      this.isUrlBeingTested = false;
      return result;
    },
  },
});