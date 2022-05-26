<template>
  <q-list separator class="q-mt-md todo-list">
    <q-item
      v-for="element in todoElements"
      :key="element.name"
      v-bind:class="{ important: element.isImportant, isDone: element.isDone }"
      class="todo-item"
    >
      <q-item-section>
        <q-item-label v-if="element.filename != null"
          ><img
            :src="getImage(element)"
            style="width: 250px; border-radius: 5px"
        /></q-item-label>
        <q-item-label lines="1" style="font-size: 24px; font-weight: bold">{{
          element.name
        }}</q-item-label>
        <q-item-label overline>{{ element.content }}</q-item-label>
        <q-item-label
          style="
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            align-items: center;
            margin-top: 40px;
          "
        >
          <q-item-label caption>{{ element.datecreate }}</q-item-label>
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
        </q-item-label>
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
      getElement();
    });
    setInterval(async () => {
      getElement();
    }, 3000);
    const deleteTodo = async (id) => {
      await Api.deleteTODO(id);
      getElement();
    };
    const changeIsDone = async (id, isDone) => {
      const formData = new FormData();
      formData.append("id", id);
      formData.append("isDone", isDone);
      await Api.changeIsDone(formData);
      getElement();
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
      getElement();
    };
    const getElement = async () => {
      const response = await Api.getTodoElement();
      todoElements.value = response;
    };
    const getImage = (element) => {
      return `http://localhost:7104/uploads/${element.filename}`;
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
      getImage,
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
.todo-list {
  display: flex;
  flex-wrap: wrap;
  gap: 15px;
}
.todo-item {
  width: calc(20% - 15px);
  border: 2px solid rgba(0, 0, 0, 0.12);
  text-align: center;
}
</style>
