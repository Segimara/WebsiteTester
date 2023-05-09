<template>
  <h2 class="mt-5">{{ testDetails?.url }}</h2>
  <div class="mt-5">
    <h2>PERFOMANCE</h2>
    <table class="table">
      <thead>
        <tr>
          <th>Url</th>
          <th>Timing</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="result in testDetails?.testResults?.slice().sort((a, b) => a.renderTimeMilliseconds - b.renderTimeMilliseconds)">
          <td>{{ result.url }}</td>
          <td>{{ result.renderTimeMilliseconds }}</td>
        </tr>
      </tbody>
    </table>
  </div>

  <div class="mt-5">
    <h2>URLs NOT FOUND AT WEBSITE</h2>
    <table class="table">
      <thead>
        <tr>
          <th>Url</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="result in testDetails?.testResults?.slice().filter(x => x.isInSitemap && !x.isInWebsite)">
          <td>{{ result.url }}</td>
        </tr>
      </tbody>
    </table>
  </div>

  <div class="mt-5">
    <h2>URLs NOT FOUND AT SITEMAP</h2>
    <table class="table">
      <thead>
        <tr>
          <th>Url</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="result in testDetails?.testResults?.slice().filter(x => x.isInWebsite && !x.isInSitemap)">
          <td>{{ result.url }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import type { Link } from '@/models/Link';
import { useWebsiteTesterStore } from '@/stores/WebsiteTesterStore';
import { ref } from 'vue';

export default {
  name: 'TestDetailsView',
  props: ['id'],
  data() {
    return {
      testDetails: ref<Link>()
    }
  },
  created() {
    const store = useWebsiteTesterStore();
    const fetchDetatils = () => {
      store.fetchTestDetatils(this.id).then((data) => {
        this.testDetails = data;
      });
    }

    fetchDetatils();
  }
}

</script>