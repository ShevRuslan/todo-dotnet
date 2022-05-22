<template>
  <q-list separator bordered class="q-mt-md">
    <q-item
      v-for="element in todoElements"
      :key="element.name"
      clickable
      v-ripple
    >
      <q-item-section>
        <q-item-label lines="1">{{ element.name }}</q-item-label>
        <q-item-label lines="2">{{ element.content }}</q-item-label>
        <q-item-label caption>{{ element.datecreate }}</q-item-label>
      </q-item-section>
    </q-item>
  </q-list>
</template>

<script>
import { defineComponent, ref, onMounted, reactive } from "vue";
import Api from "src/services/api";
export default {
  setup() {
    let todoElements = ref([]);
    onMounted(async () => {
      const response = await Api.getTodoElement();
      todoElements.value = response;
    });
    setInterval(async () => {
      const response = await Api.getTodoElement();
      todoElements.value = response;
    }, 3000);
    return {
      todoElements,
    };
  },
};
</script>

<style></style>
