<template>
  <q-btn
    color="primary"
    label="Статистика"
    @click="windowStatics = true"
    dense
    style="width: 140px"
  />
  <q-dialog v-model="windowStatics">
    <q-card style="width: 1100px; max-width: 80vw">
      <q-list class="statics" separator>
        <q-item class="statics-element">
          <q-item-section>
            <q-item-label class="number">
              {{ staticsObject.allOfWeek }}
            </q-item-label>
            <q-item-label>Количество за неделю</q-item-label>
          </q-item-section>
        </q-item>
        <q-item class="statics-element" style="border-color: red">
          <q-item-section>
            <q-item-label class="number">
              {{ staticsObject.importantOfWeek }}
            </q-item-label>
            <q-item-label>Количество важных за неделю</q-item-label>
          </q-item-section>
        </q-item>
        <q-item class="statics-element" style="border-color: green">
          <q-item-section>
            <q-item-label class="number">
              {{ staticsObject.doneOfWeek }}
            </q-item-label>
            <q-item-label>Количество сделанных за неделю</q-item-label>
          </q-item-section>
        </q-item>
        <q-item class="statics-element">
          <q-item-section>
            <q-item-label class="number">
              {{ staticsObject.allTodo }}
            </q-item-label>
            <q-item-label>Количество за все время</q-item-label>
          </q-item-section>
        </q-item>
        <q-item class="statics-element" style="border-color: red">
          <q-item-section>
            <q-item-label class="number">
              {{ staticsObject.allImportant }}
            </q-item-label>
            <q-item-label>Количество важных за все время</q-item-label>
          </q-item-section>
        </q-item>
        <q-item class="statics-element" style="border-color: green">
          <q-item-section>
            <q-item-label class="number">
              {{ staticsObject.allDone }}
            </q-item-label>
            <q-item-label>Количество сделанных за все время</q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
    </q-card>
  </q-dialog>
</template>

<script>
import Api from "src/services/api";
import { onMounted, ref, watch, computed } from "vue";
export default {
  name: "CalendarVue",
  setup() {
    const windowStatics = ref(false);
    const staticsObject = ref({});
    onMounted(async () => {
      const response = await Api.getStatics();
      staticsObject.value = response;
    });
    watch(
      () => windowStatics.value,
      async () => {
        const response = await Api.getStatics();
        staticsObject.value = response;
      }
    );
    return {
      windowStatics,
      staticsObject,
    };
  },
};
</script>

<style lang="scss">
.statics {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  gap: 16px;
  padding: 50px;
  .statics-element {
    width: calc(33% - 16px);
    border: 2px solid rgba(0, 0, 0, 0.12);
    padding: 24px;
    font-size: 18px;
    text-align: center;
    .number {
      font-size: 44px;
      font-weight: bold;
      margin-bottom: 20px;
    }
  }
}
</style>
