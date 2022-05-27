<template>
  <q-card class="q-pa-md q-mt-md q-mb-md wrapper-sort-filter">
    <q-input outlined v-model="search" label="Поиск" dense />
    <q-select
      outlined
      v-model="sortModel"
      :options="sortOptions"
      label="Сортировка"
      class="sort"
      dense
    />
    <q-select
      outlined
      v-model="filterModel"
      :options="filterOptions"
      label="Фильтрация"
      class="filter"
      dense
    />
    <CalendarVue />
    <q-btn
      color="primary"
      label="Показать все"
      dense
      style="width: 160px"
      @click="showAll()"
    />
    <Statics />
  </q-card>
  <q-list separator class="q-mt-md todo-list">
    <q-item
      v-for="element in todoElements"
      :key="element.id"
      v-bind:class="{ important: element.isImportant, isDone: element.isDone }"
      class="todo-item"
      v-show="
        formatDateCalendar(element.datecreate) == currentCaledanDay ||
        currentCaledanDay == ''
      "
    >
      <q-item-section>
        <q-item-label v-if="element.filename != null"
          ><img
            :src="getImage(element)"
            style="width: 200px; border-radius: 5px"
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
          <q-item-label caption>{{
            formatDate(element.datecreate)
          }}</q-item-label>
          <div class="text-grey-8 q-gutter-xs">
            <q-btn
              class="gt-xs"
              size="12px"
              flat
              dense
              round
              icon="delete"
              color="negative"
              @click="deleteTodo(element.id)"
            />
            <q-btn
              class="gt-xs"
              size="12px"
              flat
              dense
              round
              icon="done"
              color="positive"
              @click="changeIsDone(element.id, !element.isDone)"
            />
            <q-btn
              size="12px"
              flat
              dense
              round
              icon="edit"
              color="primary"
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
import { useStore } from "vuex";
import CalendarVue from "./Calendar.vue";
import { ref, onMounted, computed, watch } from "vue";
import Api from "src/services/api";
import { date } from "quasar";
import Statics from "./Statics.vue";
export default {
  components: {
    CalendarVue,
    Statics,
  },
  setup() {
    const store = useStore();

    const currentCaledanDay = computed(
      () => store.getters["getCurrentCalendarDay"]
    );

    const search = ref("");

    let defaultTodo = [];

    let todoElements = ref([]);
    let windowChange = ref(false);

    let name = ref("");
    let content = ref("");
    let photo = ref("");
    let important = ref(false);
    let id = ref(0);

    let sortModel = ref("");
    let sortOptions = [
      {
        label: "Сначала новые",
        value: "new",
      },
      {
        label: "Сначала старые",
        value: "old",
      },
      {
        label: "Сначала важные",
        value: "important",
      },
      {
        label: "Сначала сделанные",
        value: "done",
      },
      {
        label: "Сбросить",
        value: "removeSort",
      },
    ];
    let filterModel = ref("");
    let filterOptions = [
      {
        label: "Только важные",
        value: "important",
      },
      {
        label: "Только сделанные",
        value: "done",
      },
      {
        label: "Только с фотографиями",
        value: "photo",
      },
      {
        label: "Сбросить",
        value: "removeFilter",
      },
    ];

    watch(
      () => search.value,
      (current, prev) => {
        searchInArray(current);
      }
    );
    watch(
      () => sortModel.value,
      (current, prev) => {
        sortItems(current);
      }
    );
    watch(
      () => filterModel.value,
      (current, prev) => {
        filterItems(current);
      }
    );
    onMounted(async () => {
      getElement();
    });
    setInterval(async () => {
      getElement();
    }, 1000);
    const searchInArray = (value) => {
      if (value == "") {
        todoElements.value = [...defaultTodo];
        if (filterModel.value != "") filterItems(filterModel.value);
        if (sortModel.value != "") sortItems(sortModel.value);
      } else {
        const arrFind = todoElements.value.filter(
          (el) =>
            el.name.indexOf(value) != -1 || el.content.indexOf(value) != -1
        );
        todoElements.value = [...arrFind];
      }
    };
    const filterItems = (current) => {
      if (current.value == "important") {
        const filterArray = defaultTodo.filter(
          (element) => element.isImportant == true
        );
        todoElements.value = filterArray;
      } else if (current.value == "done") {
        const filterArray = defaultTodo.filter(
          (element) => element.isDone == true
        );
        todoElements.value = filterArray;
      } else if (current.value == "removeFilter") {
        todoElements.value = [...defaultTodo];
        sortModel.value = "";
        filterModel.value = "";
      } else if (current.value == "photo") {
        const filterArray = defaultTodo.filter(
          (element) => element.filename != null
        );
        todoElements.value = filterArray;
      }
      if (search.value != "") searchInArray(search.value);
    };
    const sortItems = (current) => {
      todoElements.value = [...defaultTodo];
      if (filterModel.value != "") filterItems(filterModel.value);
      if (current.value == "important") {
        todoElements.value.sort((a, b) => b.isImportant - a.isImportant);
      } else if (current.value == "done") {
        todoElements.value.sort((a, b) => b.isDone - a.isDone);
      } else if (current.value == "removeSort") {
        todoElements.value = [...defaultTodo];
        sortModel.value = "";
        filterModel.value = "";
      } else if (current.value == "new") {
        todoElements.value.sort((a, b) => b.datecreate - a.datecreate);
      } else if (current.value == "old") {
        todoElements.value.sort((a, b) => a.datecreate - b.datecreate);
      }
      if (search.value != "") searchInArray(search.value);
    };
    const deleteTodo = async (id) => {
      await Api.deleteTODO(id);
      getElement();
    };
    const changeIsDone = async (id, isDone) => {
      const formData = new FormData();
      formData.append("id", id);
      formData.append("isDone", isDone);
      await Api.changeIsDone(formData);
      await getElement();
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
      getElement();
    };
    const getElement = async () => {
      const response = await Api.getTodoElement();
      todoElements.value = response;
      defaultTodo = [...response];
      if (search.value != "") searchInArray(search.value);
      if (filterModel.value != "") filterItems(filterModel.value);
      if (sortModel.value != "") sortItems(sortModel.value);
    };
    const getImage = (element) => {
      return `uploads/${element.filename}`;
    };
    const formatDate = (timestamp) => {
      return date.formatDate(timestamp * 1000, "YYYY-DD-MM HH:mm");
    };
    const formatDateCalendar = (timestamp) => {
      return date.formatDate(timestamp * 1000, "YYYY/MM/DD");
    };
    const showAll = () => {
      store.commit("addCurrentCalendarDay", "");
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
      sortOptions,
      sortModel,
      formatDate,
      filterModel,
      filterOptions,
      currentCaledanDay,
      formatDateCalendar,
      showAll,
      search,
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
  width: calc(20% - 12px);
  border: 2px solid rgba(0, 0, 0, 0.12);
  text-align: center;
}
.wrapper-sort-filter {
  display: flex;
  gap: 16px;
  .sort,
  .filter {
    width: 400px;
  }
}
</style>
