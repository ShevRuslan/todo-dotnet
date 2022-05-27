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
      <q-file outlined v-model="file" label="Файл БД" dense class="q-ml-xl" />
      <q-btn
        color="primary"
        label="Загрузить файл"
        @click="importDB"
        dense
        class="button-create"
      />
      <q-btn
        flat
        round
        color="primary"
        icon="file_download"
        @click="exportDB"
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
    let file = ref("");
    const createTODO = async () => {
      const form = new FormData();
      form.append("name", name.value);
      form.append("content", content.value);
      form.append("isImportant", important.value);
      form.append("isDone", false);
      form.append("image", photo.value);
      await Api.createTODO(form);
      name.value = "";
      content.value = "";
      important.value = false;
      photo.value = "";
    };
    const exportDB = async () => {
      await Api.exportDB();
      saveDownload("uploads/export.json", "export.json");
    };
    const createAnchor = (url, fileName) => {
      let anchor = document.createElement("a");
      anchor.href = url;
      anchor.setAttribute("download", fileName);
      anchor.className = "download-js";
      anchor.innerHTML = "downloading...";
      anchor.style.display = "none";
      anchor.addEventListener("click", (e) => e.stopPropagation(), {
        once: true,
      });

      return anchor;
    };

    const saveDownload = (url, fileName) => {
      let anchor = createAnchor(url, fileName);
      document.body.appendChild(anchor);

      setTimeout(function () {
        anchor.click();
      }, 66);

      return true;
    };
    const importDB = async () => {
      const form = new FormData();
      form.append("uploadedFile", file.value);
      await Api.importDB(form);
    };
    return {
      name,
      content,
      createTODO,
      important,
      photo,
      exportDB,
      importDB,
      file,
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
