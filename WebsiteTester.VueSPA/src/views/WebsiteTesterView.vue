<template>
    <div class="d-flex flex-row gap-3 mt-0">
        <div class="form-group d-flex align-items-center flex-row w-100">
            <label class="w-25">Enter a website</label>
            <input v-model="formUrl" type="text" class="form-control" id="url" name="url" placeholder="Enter URL">
        </div>
        <div class="p-1">
            <button type="submit" class="btn btn-primary" @click="testUrl" id="testUrlButton" :disabled="isTesting"
                data-loading-text="Loading...">
                TEST
                <span v-if="isTesting" class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
            </button>
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
                    <td>{{ new Date(link.createdOn).toLocaleTimeString('default', { hour: '2-digit', minute: '2-digit' }) +
                        " " +
                        new Date(link.createdOn).toLocaleDateString('default', {
                            day: '2-digit', month: '2-digit', year:
                                'numeric'
                        })
                    }}</td>
                    <td>
                        <RouterLink :to="{ name: 'TestDetails', params: { id: link.id } }">see details</RouterLink>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script lang="ts">
import { computed } from '@vue/reactivity';
import { useWebsiteTesterStore } from '../stores/WebsiteTesterStore';
import type { Link } from '@/models/Link';
export default {
    data() {
        const store = useWebsiteTesterStore();

        return {
            store,
            formUrl: "",
        };
    },
    computed: {
        links(): Link[] {
            return this.store.$state.links;
        },
        url (): string {
            return this.store.$state.testingUrl ?? "";
        },
        isTesting (): boolean {
            return this.store.$state.isUrlBeingTested;
        }
    },
    methods:
    {
        fetchLinks() {
            this.store.fetchLinks();
        },
        testUrl() {
            this.store.testUrl(this.formUrl).then((data) => {
                if (data) {
                    this.formUrl = "";
                    this.fetchLinks();
                }
            })
        }
    },
    mounted() {
        this.fetchLinks();
    },
}
</script>