<template>
  <q-page class="flex">
    <FormCreateVue />
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import FormCreateVue from "src/components/FormCreate.vue";
export default defineComponent({
  name: "IndexPage",
  components: {
    FormCreateVue,
  },
});
</script>
