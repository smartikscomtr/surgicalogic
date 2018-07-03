<template>
  <div>
    <v-card class="grid-card">
      <div class="page-title">
        <h2> {{ title }} </h2>

        <v-container >
          <v-card-title>
            <v-text-field v-model="search"
                          append-icon="search"
                          label="Search"
                          single-line
                          hide-details>
            </v-text-field>

            <v-spacer></v-spacer>

            <v-btn class="white--text"
                  slot="activator"
                  @click="addNewItem">
              <v-icon color="white--text">
                      add
              </v-icon>
              {{ $t("common.add") }}
            </v-btn>

          </v-card-title>

          <v-data-table :headers="headers"
                        :items="items">
            <template slot="items" slot-scope="props">

              <td v-for="(header, i) in headers"
                  :key="i">
                <template v-if="!header.isAction">
                  {{ props.item[header.value] }}
                </template>

                <template v-else>
                  <v-btn v-if="showDetail" icon class="mx-0" @click="detailItem(props.item)">
                    <v-icon color="#232222">visibility</v-icon>
                  </v-btn>
                  <v-btn v-if="showEdit" icon class="mx-0" @click="editItem(props.item)">
                    <v-icon color="#232222">edit</v-icon>
                  </v-btn>

                  <v-btn v-if="showDelete" icon class="mx-0" @click="deleteItem(props.item)">
                    <v-icon color="#232222">delete</v-icon>
                  </v-btn>
                </template>
              </td>
            </template>
          </v-data-table>
        </v-container>
      </div>
    </v-card>
  </div>
</template>

<script>
export default {
  props: {
    items: {
      type: Array,
      required: false,
      default() {
        return [];
      }
    },

    headers: {
      type: Array,
      required: false
    },

    title: {
      type: String,
      required: false
    },

    showDetail: {
      type: Boolean,
      required: false
    },

    showEdit: {
      type: Boolean,
      required: false
    },

    showDelete: {
      type: Boolean,
      required: false
    }
  },

  data() {
    return {
      search: ""
    };
  },

  methods: {
    addNewItem() {
      const vm = this;

      vm.$emit("newaction");
    },

    detailItem(item) {
      const vm = this;

      vm.$emit("detail", item);
    },

    editItem(item) {
      const vm = this;

      vm.$emit("edit", item);
    },

    deleteItem(item) {
      const vm = this;

      vm.$emit("deleteitem", item);
    }
  }
};
</script>

<style>

.grid-card.card {
  box-shadow: inherit;
  height: 100vh !important;
}
.grid-card .page-title h2 {
  padding-left: 16px;
  padding-top: 20px;
}
.datatable__actions {
  background-color: #f8f8f8 !important;
  padding-top: 20px;
}
.datatable__actions__range-controls {
  margin-right: 15px;
}
.white--text {
  width: 158px;
  margin-right: 0;
  background-color: #ff7107 !important;
}
.table__overflow {
  margin-top: 36px;
}

table.table thead tr {
  height: 70px;
}

table.table thead th {
  font-weight: bold;
  font-size: initial;
}

table.table thead td {
  font-weight: 400;
  font-size: 75%;
}

table.table tbody td:last-child {
  text-align: right;
}
tr:nth-child(even) {
  background-color: #f2f2f2;
}
.headline-wrap {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
}
.card {
  border-radius: 8px;
  background-color: #fff;
  position: relative;
  margin-top: 0;
  margin-bottom: 0;
  padding: 12px 42px;
}
.card__title {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-direction: row;
}
.text {
  flex: 1;
  font-size: 18px;
  color: #000;
  margin: 0;
}
.btnSave {
  padding: 0;
  margin: 0;
  min-width: 140px;
  background-color: #ff7107 !important;
  height: 40px;
  font-size: 15px;
}
.btnSave .btn__content{ color: #fff;}
.headline-wrap .btn--active .btn__content:before,
.headline-wrap .btn:focus .btn__content:before,
.headline-wrap .btn:hover .btn__content:before {
  background-color: transparent;
}
.headline-wrap .backBtn {
  position: absolute;
  left: 20px;
}
.headline-wrap .backBtn i {
  color: #000 !important;
}
.label {
  font-size: 12px;
  line-height: 1.33;
  text-align: left;
  color: #464646;
}
.value {
  font-size: 16px;
  line-height: 1.5;
  text-align: left;
  color: #000;
}
.dialog.dialog--active {
  max-width: 600px;
}
</style>
