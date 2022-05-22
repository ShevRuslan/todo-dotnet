<template>
  <q-card class="wrapper-form">
    <q-card-section class="form">
      <q-input outlined v-model="name" label="Название" dense />
      <q-input outlined v-model="content" label="Контент" dense />
      <q-file outlined v-model="photo" label="Фотография" dense />
      <q-checkbox v-model="important" label="Важное" />
      <q-btn
        color="primary"
        label="Создать"
        @click="createTODO"
        dense
        class="button-create"
      />
    </q-card-section>
  </q-card>
</template>

<script>
import { defineComponent, ref } from "vue";
import Api from "../services/api";
export default defineComponent({
  name: "FormCreate",
  setup() {
    let name = ref("");
    let content = ref("");
    let important = ref(false);
    let photo = ref("");
    const createTODO = async () => {
      const form = new FormData();
      form.append("name", name.value);
      form.append("content", content.value);
      form.append("important", important.value);
      form.append("image", photo.value);
      const response = await Api.createTODO(form);
    };
    return {
      name,
      content,
      createTODO,
      important,
      photo,
    };
  },
});
</script>

<style lang="scss">
.wrapper-form {
  width: 100%;
  padding: 15px;
  max-height: 100px;
  .form {
    display: flex;
    gap: 16px;
    .button-create {
      width: 140px;
    }
  }
}
</style>
