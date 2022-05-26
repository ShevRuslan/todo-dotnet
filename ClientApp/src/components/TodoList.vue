<template>
  <q-list separator bordered class="q-mt-md">
    <q-item
      v-for="element in todoElements"
      :key="element.name"
      v-bind:class="{ important: element.isImportant, isDone: element.isDone }"
    >
      <q-item-section>
        <q-item-label lines="1">{{ element.name }}</q-item-label>
        <q-item-label lines="2">{{ element.content }}</q-item-label>
        <q-item-label caption>{{ element.datecreate }}</q-item-label>
      </q-item-section>
      <q-item-section side>
        <div class="text-grey-8 q-gutter-xs">
          <q-btn
            class="gt-xs"
            size="12px"
            flat
            dense
            round
            icon="delete"
            @click="deleteTodo(element.id)"
          />
          <q-btn
            class="gt-xs"
            size="12px"
            flat
            dense
            round
            icon="done"
            @click="changeIsDone(element.id, !element.isDone)"
          />
          <q-btn
            size="12px"
            flat
            dense
            round
            icon="edit"
            @click="openWindow(element)"
          />
        </div>
      </q-item-section>
    </q-item>
  </q-list>
  <q-dialog v-model="windowChange">
    <q-card style="width: 700px; max-width: 80vw">
      <q-card-section>
        <div class="text-h6">Редактирование</div>
      </q-card-section>

      <q-card-section class="q-pt-none">
        <q-input
          outlined
          v-model="name"
          label="Название"
          dense
          class="q-mt-md"
        />
        <q-input
          outlined
          v-model="content"
          label="Контент"
          dense
          class="q-mt-md"
        />
        <q-file
          outlined
          v-model="photo"
          label="Фотография"
          dense
          class="q-mt-md"
        />
        <q-checkbox v-model="important" label="Важное" class="q-mt-md" />
      </q-card-section>

      <q-card-actions align="center" class="bg-white text-teal">
        <q-btn
          color="primary"
          label="OK"
          v-close-popup
          style="width: 100%"
          @click="edit"
        />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script>
import { defineComponent, ref, onMounted, reactive } from "vue";
import Api from "src/services/api";
export default {
  setup() {
    let todoElements = ref([]);
    let windowChange = ref(false);

    let name = ref("");
    let content = ref("");
    let photo = ref("");
    let important = ref(false);
    let id = ref(0);

    onMounted(async () => {
      const response = await Api.getTodoElement();
      todoElements.value = response;
    });
    setInterval(async () => {
      const response = await Api.getTodoElement();
      todoElements.value = response;
    }, 1000);
    const deleteTodo = async (id) => {
      await Api.deleteTODO(id);
      const response = await Api.getTodoElement();
      todoElements.value = response;
    };
    const changeIsDone = async (id, isDone) => {
      const formData = new FormData();
      formData.append("id", id);
      formData.append("isDone", isDone);
      await Api.changeIsDone(formData);
      const response = await Api.getTodoElement();
      todoElements.value = response;
    };
    const openWindow = async (element) => {
      windowChange.value = true;
      name.value = element.name;
      content.value = element.content;
      photo.value = element.photo;
      important.value = element.isImportant;
      id.value = element.id;
    };
    const edit = async () => {
      const form = new FormData();
      form.append("name", name.value);
      form.append("content", content.value);
      form.append("isImportant", important.value);
      form.append("isDone", false);
      form.append("image", photo.value);
      form.append("id", id.value);
      await Api.editTodo(form);
      await Api.changeIsDone(formData);
      const response = await Api.getTodoElement();
      todoElements.value = response;
    };
    return {
      todoElements,
      deleteTodo,
      changeIsDone,
      windowChange,
      name,
      content,
      photo,
      important,
      openWindow,
      edit,
    };
  },
};
</script>

<style lang="scss">
.important {
  border: 2px solid red !important;
}
.isDone {
  border: 2px solid green !important;
}
</style>
