<template>
    <div class="d-flex flex-row gap-3 mt-5">
        <div class="form-group d-flex align-items-center flex-row w-100">
            <label class="w-25">Enter a website</label>
            <input v-model="formUrl" type="text" class="form-control" id="url" name="url" placeholder="Enter URL">
        </div>
        <div class="p-1">
            <button type="submit" class="btn btn-primary" @click="testUrl" id="testUrlButton">TEST</button>
        </div>
    </div>
    <div>
        <h2 class="mt-5">Test Results</h2>
        <table class="table mt-5">
            <thead>
                <tr>
                    <th scope="col">Website</th>
                    <th scope="col">Date</th>
                    <th scope="col"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="link in links" :key="link.id">
                    <td>{{ link.url }}</td>
                    <td>{{ link.createdOn }}</td>
                    <td>
                        <RouterLink :to="{name: 'TestDetails', params: {id: link.id}}">see details</RouterLink>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useWebsiteTesterStore } from '../stores/WebsiteTesterStore';
import { computed, ref } from '@vue/reactivity';

export default defineComponent({

    setup() {
        const store = useWebsiteTesterStore();
        const testUrl = () => {
            store.testUrl(formUrl.value);
        }
        const fetchLinks = () => {
            store.fetchLinks();
        }
        const links = computed(() => store.$state.links);

        let isTesting = computed(() => store.$state.isUrlBeingTested);

        const url = store.$state.testingUrl;
        const formUrl = ref("");
        return { links, formUrl, url, isTesting, testUrl, fetchLinks };
    },
    created() {
        this.fetchLinks();
    },
});
</script>