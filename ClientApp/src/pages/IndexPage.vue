<template>
  <q-page class="q-pa-md">
    <FormCreateVue />
    <TodoList />
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import FormCreateVue from "src/components/FormCreate.vue";
import TodoList from "src/components/TodoList.vue";
export default defineComponent({
  name: "IndexPage",
  components: {
    FormCreateVue,
    TodoList,
  },
});
</script>
