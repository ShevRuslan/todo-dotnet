<template>
  <q-btn
    color="primary"
    label="Календарь"
    @click="calendarWindow = true"
    dense
    style="width: 140px"
  />
  <q-dialog v-model="calendarWindow">
    <q-date v-model="currentDate" :events="events" :event-color="orange" />
  </q-dialog>
</template>

<script>
import { useStore } from "vuex";
import Api from "src/services/api";
import { onMounted, ref, watch, computed } from "vue";
import { date } from "quasar";
export default {
  name: "CalendarVue",
  setup() {
    const store = useStore();
    const currentDate = ref("");
    const events = ref([]);
    const calendarWindow = ref(false);
    onMounted(async () => {
      const dates = await Api.getAllDates();
      dates.forEach((date) => {
        events.value.push(formatDate(date));
      });
      currentDate.value = events.value[0];
    });
    watch(
      () => currentDate.value,
      (current, prev) => {
        console.log(current);
        store.commit("addCurrentCalendarDay", current);
        calendarWindow.value = false;
      }
    );
    const formatDate = (timestamp) => {
      return date.formatDate(timestamp * 1000, "YYYY/MM/DD");
    };
    return {
      currentDate,
      events,
      calendarWindow,
    };
  },
};
</script>

<style></style>
